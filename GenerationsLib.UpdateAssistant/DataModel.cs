using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace GenerationsLib.UpdateAssistant
{
    public static class DataModel
    {
        public static string EXEPath { get => System.Reflection.Assembly.GetEntryAssembly().Location; }
        public static string AssistantsFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GenerationsLib Update Assistant", "assistants"); } }
        public static string CacheFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GenerationsLib Update Assistant", "cache"); } }
        public static string TempFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GenerationsLib Update Assistant", "temp"); } }

        public static List<UpdateAssistant> Assistants { get; set; }

        public static void GetUpdateAssistants()
        {
            //Clear or Create List
            if (Assistants != null) Assistants.Clear();
            else Assistants = new List<UpdateAssistant>();


            //Create if Missing Directory and Get All Files
            if (!Directory.Exists(AssistantsFolder)) Directory.CreateDirectory(AssistantsFolder);
            DirectoryInfo d = new DirectoryInfo(AssistantsFolder);

            //Process all JSON Files with Valid Data
            string searchPattern = "*.json";
            foreach (FileInfo file in d.GetFiles(searchPattern))
            {
                UpdateAssistant assistant = new UpdateAssistant(file.FullName);
                if (assistant != null) Assistants.Add(assistant);
            }
        }
    }
}
