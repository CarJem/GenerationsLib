using Microsoft.Build.Framework;
using MSBuildTask = Microsoft.Build.Utilities.Task;
using System.IO;
using System;
using System.Linq;

namespace GenerationsLib.MSBuild
{
    public class MoveDirectory : MSBuildTask
    {
        public static void MoveDirectoryFunc(string source, string target)
        {
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');

            if (Directory.Exists(sourcePath))
            {
                var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                                     .GroupBy(s => Path.GetDirectoryName(s));
                foreach (var folder in files)
                {
                    var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                    Directory.CreateDirectory(targetFolder);
                    foreach (var file in folder)
                    {
                        var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                        File.Move(file, targetFile);
                    }
                }
                Directory.Delete(source, true);
            }

        }
        public override bool Execute()
        {
            try
            {
                var Source = Path.GetFullPath(SourceFolder.ToString());
                var Destination = Path.Combine(DestinationFolder.ToString(), new DirectoryInfo(Source).Name);
                try
                {

                    MoveDirectoryFunc(Source, Destination);
                }
                catch (Exception ex2)
                {
                    Log.LogError(ex2.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                return false;
            }
        }

        [Required]
        public bool OverwriteReadOnlyFiles { get; set; } = false;

        [Required]
        public ITaskItem SourceFolder { get; set; }

        [Required]
        public ITaskItem DestinationFolder { get; set; }
    }
}
