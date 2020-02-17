using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GenerationsLib.UpdateAssistant.PublishUI
{
    public partial class Publisher : Form
    {
        public UpdateAssistant ItemBeingPublished { get; set; }
        public Publisher(string value)
        {
            InitializeComponent();
            try
            {
                if (Directory.Exists(DataModel.TempFolder)) GenerationsLib.Core.FileHelpers.DeleteReadOnlyDirectory(DataModel.TempFolder);
                Directory.CreateDirectory(DataModel.TempFolder);
            }
            catch
            {
                MessageBox.Show("Please Manually Clean the Temp Folder!");
            }

            DataModel.GetUpdateAssistants();
            if (DataModel.Assistants.Exists(x => x.Details.ID == value))
            {
                ItemBeingPublished = DataModel.Assistants.FirstOrDefault(x => x.Details.ID == value);
                PublishingChain();
            }
        }

        #region File Generation / String Formating

        private void GenerateUpdateConfig(UpdateAssistant.Structure.Metadata meta)
        {
            if (meta != null)
            {
                try
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(meta, Newtonsoft.Json.Formatting.Indented);
                    JObject rawData = Newtonsoft.Json.Linq.JObject.Parse(data);
                    JObject optimizedData = new JObject();
                    optimizedData.Add("Metadata", rawData);

                    if (!Directory.Exists(DataModel.TempFolder)) Directory.CreateDirectory(DataModel.TempFolder);
                    string filePath = Path.Combine(DataModel.TempFolder, string.Format("{0}_Updates.json", ItemBeingPublished.Details.ID));
                    File.WriteAllText(filePath, optimizedData.ToString());

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string GenerateTemporaryBatchScript(string scriptData)
        {
            if (!Directory.Exists(DataModel.TempFolder)) Directory.CreateDirectory(DataModel.TempFolder);
            string filePath = Path.Combine(DataModel.TempFolder, "temp_script.bat");
            File.WriteAllText(filePath, scriptData);
            return filePath;
        }

        #endregion

        private bool ChangesDialog()
        {
            var result = MessageBox.Show("Would You Like to Make Edits to the Config Before Publishing", "Make Edits?", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                string id = ItemBeingPublished.Details.ID;
                DeploymentDialog editor = new DeploymentDialog();
                editor.ShowConfigDialog(ItemBeingPublished);
                editor.ShowDialog();
                DataModel.GetUpdateAssistants();
                ItemBeingPublished = DataModel.Assistants.FirstOrDefault(x => x.Details.ID == id);
                return ChangesDialog();
            }
            else if (result == DialogResult.Cancel)
            {
                return false;
            }
            else return true;
        }

        private void PublishingChain()
        {
            if (ChangesDialog() == true)
            {
                var versionSelector = new PublishUI.PublisherVersionSelectionDialog();
                if (versionSelector.ShowSelectionDialog(ItemBeingPublished) == DialogResult.OK)
                {
                    GenerateUpdateConfig(versionSelector.SelectedVersion);
                    string scriptFile = GenerateTemporaryBatchScript(ItemBeingPublished.Details.PublishScriptCode);
                    RunScript(scriptFile);
                    DeploymentDialog deploymentDialog = new DeploymentDialog();
                    deploymentDialog.ShowConfigDialog(ItemBeingPublished);
                    deploymentDialog.ShowDialog();
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else Environment.Exit(0);


        }

        private void RunScript(string filePath)
        {
            string command = string.Format("/k cmd.exe /c \"{0}\"", filePath);
            Console.WriteLine(command);
            var processInfo = new ProcessStartInfo("cmd.exe");
            processInfo.CreateNoWindow = false;
            processInfo.Arguments = command;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = false;
            processInfo.RedirectStandardOutput = false;
            var process = Process.Start(processInfo);
            process.WaitForExit();
            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }
    }
}
