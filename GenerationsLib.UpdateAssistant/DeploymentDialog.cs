using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerationsLib.UpdateAssistant
{
    public partial class DeploymentDialog : Form
    {
        public DeploymentDialog()
        {
            InitializeComponent();
            UpdateButtons();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            //this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private UpdateAssistant EditedItem;
        public void ShowConfigDialog(UpdateAssistant item)
        {
            EditedItem = item;
            SetupControls();
            var result = this.ShowDialog();
        }

        private void SetupControls()
        {
            foreach (var entry in EditedItem.Details.SitesToPublishTo)
            {
                listBox1.Items.Add(entry);
            }
            foreach (var entry in EditedItem.Details.DownloadHosts)
            {
                listBox2.Items.Add(entry);
            }
            foreach (var entry in EditedItem.Details.VersionMetadata)
            {
                listBox3.Items.Add(entry);
            }
            foreach (var entry in EditedItem.Details.PlacesToPost)
            {
                listBox4.Items.Add(entry);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                try
                {
                    string url = (listBox1.SelectedItem as UpdateAssistant.Structure.ExtendedString).String2;
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (listBox1.SelectedItem != null)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }

            if (listBox2.SelectedItem != null)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }


            if (listBox4.SelectedItem != null)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
            PraseMetadata();
            PrasePostData();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void PraseMetadata()
        {
            richTextBox1.ResetText();
            richTextBox3.ResetText();
            if (listBox3.SelectedItem != null)
            {
                try
                {
                    UpdateAssistant.Structure.Metadata meta = (listBox3.SelectedItem as UpdateAssistant.Structure.Metadata);
                    string changelog = meta.Details;
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(meta, Newtonsoft.Json.Formatting.Indented);
                    richTextBox1.Text = data;
                    richTextBox3.Text = changelog;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void PrasePostData()
        {
            richTextBox2.ResetText();
            if (listBox4.SelectedItem != null)
            {
                try
                {
                    UpdateAssistant.Structure.SiteForPostData meta = (listBox4.SelectedItem as UpdateAssistant.Structure.SiteForPostData);
                    string data = meta.Notes;
                    richTextBox2.Text = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                try
                {
                    string url = (listBox2.SelectedItem as UpdateAssistant.Structure.ExtendedString).String2;
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                try
                {
                    string url = (listBox4.SelectedItem as UpdateAssistant.Structure.SiteForPostData).URL;
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }
    }
}
