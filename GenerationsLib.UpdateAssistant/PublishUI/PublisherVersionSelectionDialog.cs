using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerationsLib.UpdateAssistant.PublishUI
{
    public partial class PublisherVersionSelectionDialog : Form
    {
        public List<UpdateAssistant.Structure.Metadata> Versions { get; set; }
        public UpdateAssistant.Structure.Metadata SelectedVersion { get; set; }
        public PublisherVersionSelectionDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowSelectionDialog(UpdateAssistant updateAssistant)
        {
            Versions = updateAssistant.Details.VersionMetadata;
            versionListBox.DataSource = Versions;
            return this.ShowDialog();
        }

        private void versionListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (versionListBox.SelectedItem != null)
            {
                SelectedVersion = (versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata);
                selectButton.Enabled = true;
            }
            else
            {
                SelectedVersion = null;
                selectButton.Enabled = false;
            }
        }
    }
}
