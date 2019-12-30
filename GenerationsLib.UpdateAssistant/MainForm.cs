﻿using System;
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
    public partial class MainForm : Form
    {
        private string EXEPath { get => System.Reflection.Assembly.GetEntryAssembly().Location; }
        private string AssistantsFolder { get { return Path.Combine(Path.GetDirectoryName(EXEPath), "assistants"); } }
        public static List<UpdateAssistant> Assistants { get; set; }
        public MainForm()
        {
            InitializeComponent();
            RefreshTool();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterParent;
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
                if (assistant != null) Assistants.Add(assistant);
            }
        }

        bool IsValidFilename(string testName)
        {
            if (testName == "") return false;
            Regex containsABadCharacter = new Regex("["
                  + Regex.Escape(System.IO.Path.GetInvalidFileNameChars().ToString()) + "]");
            if (containsABadCharacter.IsMatch(testName)) { return false; };

            // other checks for UNC, drive-path format, etc

            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(AssistantsFolder)) Directory.CreateDirectory(AssistantsFolder);
            UpdateAssistant new_assistant = new UpdateAssistant();
            string filename = "";
            while (!IsValidFilename(filename) || File.Exists(filename + ".json"))
            {
                filename = Essy.Tools.InputBox.InputBox.ShowInputBox("Provide a Filename for this item"); ;
            }
            string name = Essy.Tools.InputBox.InputBox.ShowInputBox("Provide a Name for this Item");
            new_assistant.Details.Name = name;
            string filePath = Path.Combine(AssistantsFolder, filename + ".json");
            new_assistant.Save(filePath);
            RefreshTool();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                removeButton.Enabled = true;
                editButton.Enabled = true;
                runButton.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
                editButton.Enabled = false;
                runButton.Enabled = false;
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ConfigurationManager editor = new ConfigurationManager();
                Assistants[Assistants.IndexOf(listBox1.SelectedItem as UpdateAssistant)] = editor.ShowConfigDialog(listBox1.SelectedItem as UpdateAssistant);
                Assistants[Assistants.IndexOf(listBox1.SelectedItem as UpdateAssistant)].Save();
                RefreshTool();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                File.Delete(Assistants[Assistants.IndexOf(listBox1.SelectedItem as UpdateAssistant)].Path);
                RefreshTool();
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            DeploymentDialog editor = new DeploymentDialog();
            editor.ShowConfigDialog(listBox1.SelectedItem as UpdateAssistant);
        }
    }
}
