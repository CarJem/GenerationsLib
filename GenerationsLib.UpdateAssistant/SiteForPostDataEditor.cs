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
    public partial class SiteForPostDataEditor : Form
    {
        private UpdateAssistant EditedItem;

        public SiteForPostDataEditor()
        {
            InitializeComponent();
            UpdateSelectedButtons();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            //this.MaximumSize = this.Size;
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

        private void UpdateBindings()
        {
            if (listBox.DataSource == null) listBox.DataSource = bindingSource1; 

            if (EditedItem != null)
            {
                bindingSource1.DataSource = EditedItem.Details.PlacesToPost;
            }

        }

        private void UpdateSelectedBindings()
        {
            downloadURLTextBox.DataBindings.Clear();
            itemNameTextbox.DataBindings.Clear();
            descriptionTextBox.DataBindings.Clear();

            if (listBox.SelectedItem != null && listBox.SelectedItem is UpdateAssistant.Structure.SiteForPostData)
            {
                itemNameTextbox.DataBindings.Add(new Binding("Text", (listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "Name"));
                descriptionTextBox.DataBindings.Add(new Binding("Text", (listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "Notes"));
                downloadURLTextBox.DataBindings.Add(new Binding("Text", (listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "URL"));
            }

        }

        private void UpdateSelectedButtons()
        {
            moveUpButton.Visible = false;
            moveDownButton.Visible = false;

            if (listBox.SelectedItem != null)
            {
                removeButton.Enabled = true;
                moveUpButton.Enabled = true;
                moveDownButton.Enabled = true;

                downloadURLTextBox.Enabled = true;
                itemNameTextbox.Enabled = true;
                descriptionTextBox.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
                moveUpButton.Enabled = false;
                moveDownButton.Enabled = false;

                downloadURLTextBox.Enabled = false;
                itemNameTextbox.Enabled = false;
                descriptionTextBox.Enabled = false;
            }
        }

        private void versionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedBindings();
            UpdateSelectedButtons();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var newItem = new UpdateAssistant.Structure.SiteForPostData();
            newItem.Name = "NULL";
            bindingSource1.Add(newItem);
            UpdateBindings();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            bindingSource1.Remove(listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData);
            UpdateBindings();
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {

        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {

        }

        private void itemNameTextbox_TextChanged(object sender, EventArgs e)
        {
            listBox.Refresh();
        }
    }
}
