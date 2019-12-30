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
    public partial class ConfigurationManager : Form
    {
        private UpdateAssistant EditedItem;
        public ConfigurationManager()
        {
            InitializeComponent();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public UpdateAssistant ShowConfigDialog(UpdateAssistant assistant)
        {
            EditedItem = assistant;
            UpdateBindings();
            var result = this.ShowDialog();
            return EditedItem;
        }

        private void configButton4_Click(object sender, EventArgs e)
        {
            SiteForPostDataEditor editor = new SiteForPostDataEditor();
            EditedItem = editor.ShowConfigDialog(EditedItem);
            UpdateBindings();
        }

        private void configButton3_Click(object sender, EventArgs e)
        {
            VersionMetadataEditor editor = new VersionMetadataEditor();
            EditedItem = editor.ShowConfigDialog(EditedItem);
            UpdateBindings();
        }

        private void configButton2_Click(object sender, EventArgs e)
        {
            SimpleListEditor editor = new SimpleListEditor();
            EditedItem.Details.DownloadHosts = editor.ShowConfigDialog(EditedItem.Details.DownloadHosts);
            UpdateBindings();
        }

        private void configButton1_Click(object sender, EventArgs e)
        {
            SimpleListEditor editor = new SimpleListEditor();
            EditedItem.Details.SitesToPublishTo = editor.ShowConfigDialog(EditedItem.Details.SitesToPublishTo);
            UpdateBindings();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateBindings()
        {
            textBox1.DataBindings.Clear();
            if (EditedItem != null)
            {
                textBox1.DataBindings.Add(new Binding("Text", EditedItem, "Details.Name"));
            }

        }

        private void ConfigurationManager_VisibleChanged(object sender, EventArgs e)
        {
            //UpdateBindings();
        }
    }
}
