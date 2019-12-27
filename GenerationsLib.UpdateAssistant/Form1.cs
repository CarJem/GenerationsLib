using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GenerationsLib.UpdateAssistant
{
    public partial class Form1 : Form
    {
        private string EXEPath { get => System.Reflection.Assembly.GetEntryAssembly().Location; }
        private string AssistantsFolder { get { return Path.Combine(Path.GetDirectoryName(EXEPath), "assistants"); } }
        public static List<UpdateAssistant> Assistants { get; set; }
        public Form1()
        {
            InitializeComponent();
            RefreshTool();
        }

        private void RefreshTool()
        {
            listBox1.DataSource = null;
            GetUpdateAssistants();
            listBox1.DataSource = Assistants;
            listBox1.Refresh();
        }

        private void GetUpdateAssistants()
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
            }
        }
    }
}
