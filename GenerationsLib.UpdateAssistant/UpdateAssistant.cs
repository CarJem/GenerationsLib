using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GenerationsLib.UpdateAssistant
{
    public class UpdateAssistant
    {
        public class Structure
        {
            public string Name { get; set; } = "NULL";
            public string ID { get; set; } = "NULL";
            public List<ExtendedString> DownloadHosts { get; set; } = new List<ExtendedString>();
            public List<ExtendedString> SitesToPublishTo { get; set; } = new List<ExtendedString>();
            public List<SiteForPostData> PlacesToPost { get; set; } = new List<SiteForPostData>();
            public List<Metadata> VersionMetadata { get; set; } = new List<Metadata>();
            public string PublishScriptCode { get; set; } = "";

            public class ExtendedString
            {
                public ExtendedString(string str1, string str2)
                {
                    String1 = str1;
                    String2 = str2;
                }
                public ExtendedString()
                {

                }
                public override string ToString()
                {
                    return string.Format("{0}", String1);
                }
                public string String1 { get; set; } = "";
                public string String2 { get; set; } = "";
            }

            public class SiteForPostData
            {
                public override string ToString()
                {
                    return Name;
                }
                public string Name { get; set; } = "";
                public string URL { get; set; } = "";
                public string Notes { get; set; } = "";
            }

            public class Metadata
            {
                public override string ToString()
                {
                    return Version;
                }
                public string Version { get; set; } = "";
                public string Details { get; set; } = "";
                public string DownloadURL { get; set; } = "";
            }
        }

        public override string ToString()
        {
            return Details.Name;
        }

        public Structure Details { get; set; } = new Structure();
        private string FileLocation { get; set; } = "";
        public string Path { get => FileLocation; }
        private bool isLocationSet { get; set; } = false;

        public UpdateAssistant(string filePath)
        {
            Load(filePath);
        }

        public UpdateAssistant()
        {
            Details = new Structure();
        }

        public void Load(string filePath)
        {
            SetLocation(filePath);
            if (!File.Exists(FileLocation)) File.Create(FileLocation).Close();
            string text = File.ReadAllText(FileLocation);

            try
            {
                Structure result = JsonConvert.DeserializeObject<Structure>(text, new JsonSerializerSettings() { });
                if (result != null) Details = result;
                else Details = new Structure();
            }
            catch
            {
                Details = new Structure();
            }


        }

        private void SetLocation(string filePath)
        {
            FileLocation = filePath;
            isLocationSet = true;
        }


        public void Save(string filePath = null)
        {
            try
            {
                if (filePath != null) SetLocation(filePath);
                if (isLocationSet)
                {
                    if (!File.Exists(FileLocation)) File.Create(FileLocation).Close();
                    string text = JsonConvert.SerializeObject(Details, Formatting.Indented);
                    File.WriteAllText(FileLocation, text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
