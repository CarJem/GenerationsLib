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
            string filePath = Path.Combine(DataModel.TempFolder, "temp_script.bat");
            File.WriteAllText(filePath, scriptData);
            return filePath;
        }

        #endregion


        private void PublishingChain()
        {
            MessageBox.Show("Click OK when the rest of the Post-Build Operations have Completed to Proceed!");
            var versionSelector = new PublishUI.PublisherVersionSelectionDialog();
            if (versionSelector.ShowSelectionDialog(ItemBeingPublished) == DialogResult.OK)
            {
                GenerateUpdateConfig(versionSelector.SelectedVersion);
                string scriptFile = GenerateTemporaryBatchScript(ItemBeingPublished.Details.PublishScriptCode);
                RunScript(scriptFile);
            }
            DeploymentDialog deploymentDialog = new DeploymentDialog();
            deploymentDialog.ShowConfigDialog(ItemBeingPublished);
            deploymentDialog.ShowDialog();
            Environment.Exit(0);
        }

        private void RunScript(string filePath)
        {
            var processInfo = new ProcessStartInfo(filePath);
            processInfo.CreateNoWindow = false;
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
