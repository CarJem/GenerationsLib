using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GenerationsLib.Versioning
{
    class Program
    {
        static BuildList BuildList;
        static string GetVersion(bool isFile, string buildNumber)
        {
            string versionStart;
            string versionEnd;

            if (isFile)
            {
                versionStart = "[assembly: AssemblyFileVersion(\"";
                versionEnd = "\")]";
            }
            else
            {
                versionStart = "[assembly: AssemblyVersion(\"";
                versionEnd = "\")]";
            }

            return string.Format("{0}{1}.{2}{3}", versionStart, DateTime.Now.Date.ToString("yy.MM.dd"), buildNumber, versionEnd);


        }

        static void GetBuildList()
        {
            string exeLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exeLocation);
            string buildListFile = Path.Combine(exeDirectory, "build_list.json");
            if (!File.Exists(buildListFile)) File.Create(buildListFile).Close();

            string data = File.ReadAllText(buildListFile);

            BuildList = Newtonsoft.Json.JsonConvert.DeserializeObject<BuildList>(data);
            if (BuildList == null) BuildList = new BuildList();
        }

        static void SaveBuildList()
        {
            string exeLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            string exeDirectory = Path.GetDirectoryName(exeLocation);
            string buildListFile = Path.Combine(exeDirectory, "build_list.json");
            if (!File.Exists(buildListFile)) File.Create(buildListFile);

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(BuildList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(buildListFile, output);
        }

        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                GetBuildList();
                string file = args[0];
                string name = args[1];

                bool ignoreFile = false;
                bool ignoreInternal = false;

                if (args.Length >= 3) bool.TryParse(args[2], out ignoreFile);
                if (args.Length >= 4) bool.TryParse(args[3], out ignoreInternal);

                var fileVersionRx = new Regex(@"\[assembly: AssemblyFileVersion\((.*)\)\]");
                var versionRx = new Regex(@"\[assembly: AssemblyVersion\((.*)\)\]");

                string buildNum = GetBuildNumber(name);

                if (File.Exists(file))
                {
                    if (!ignoreFile)
                    {
                        File.WriteAllLines(file,
                            File.ReadAllLines(file).Select(s =>
                                fileVersionRx.Replace(s, GetVersion(true, buildNum))
                            )
                        );
                    }
                    if (!ignoreInternal)
                    {
                        File.WriteAllLines(file,
                            File.ReadAllLines(file).Select(s =>
                                versionRx.Replace(s, GetVersion(false, buildNum))
                            )
                        );
                    }
                }
                SaveBuildList();
                BuildList = null;
            }

        }

        static string GetBuildNumber(string name)
        {
            if (BuildList.BuiltAssemblies != null)
            {
                if (BuildList.BuiltAssemblies.Exists(x => x.Identifier == name))
                {
                    int indexOfEntry = BuildList.BuiltAssemblies.IndexOf(BuildList.BuiltAssemblies.FirstOrDefault(x => x.Identifier == name));
                    if (BuildList.BuiltAssemblies[indexOfEntry].LastBuilt != null)
                    {
                        if (DateTime.Compare(DateTime.Now.Date, BuildList.BuiltAssemblies[indexOfEntry].LastBuilt) > 0)
                        {
                            BuildList.BuiltAssemblies[indexOfEntry].LastBuilt = DateTime.Now.Date;
                            BuildList.BuiltAssemblies[indexOfEntry].BuildNumber = 0;
                        }
                        else
                        {
                            BuildList.BuiltAssemblies[indexOfEntry].LastBuilt = DateTime.Now.Date;
                            BuildList.BuiltAssemblies[indexOfEntry].BuildNumber = BuildList.BuiltAssemblies[indexOfEntry].BuildNumber + 1;
                        }
                    }
                    else
                    {
                        BuildList.BuiltAssemblies[indexOfEntry].LastBuilt = DateTime.Now.Date;
                        BuildList.BuiltAssemblies[indexOfEntry].BuildNumber = 0;
                    }

                    return BuildList.BuiltAssemblies[indexOfEntry].BuildNumber.ToString();
                }
                else
                {
                    var entry = new BuildList.BuiltAssembly();

                    entry.LastBuilt = DateTime.Now.Date;
                    entry.BuildNumber = 0;
                    entry.Identifier = name;

                    BuildList.BuiltAssemblies.Add(entry);

                    return entry.BuildNumber.ToString();
                }
            }
            else
            {
                BuildList.BuiltAssemblies = new List<BuildList.BuiltAssembly>();

                var entry = new BuildList.BuiltAssembly();

                entry.LastBuilt = DateTime.Now.Date;
                entry.BuildNumber = 0;
                entry.Identifier = name;

                BuildList.BuiltAssemblies.Add(entry);

                return entry.BuildNumber.ToString();

            }

        }
    }
}
