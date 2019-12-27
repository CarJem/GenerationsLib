using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenerationsLib.UpdateAssistant
{
    public class UpdateAssistant
    {
        public Metadata VersionMetadata { get; set; }
        public List<string> SitesToPublishTo { get; set; }
        public class Metadata
        {
            public string Version { get; set; } = "";
            public string Details { get; set; } = "";
            public string DownloadURL { get; set; } = "";
        }
        public UpdateAssistant(string filePath)
        {
            Load(filePath);
        }

        public UpdateAssistant()
        {

        }

        public void Load(string filePath)
        {

        }


        public void Save()
        {

        }
    }
}
