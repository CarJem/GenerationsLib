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
    public partial class SimpleListEditor : Form
    {
        public SimpleListEditor()
        {
            InitializeComponent();
            UpdateSelectedButtons();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private List<UpdateAssistant.Structure.ExtendedString> EditedItem;

        public List<UpdateAssistant.Structure.ExtendedString> ShowConfigDialog(List<UpdateAssistant.Structure.ExtendedString> item)
        {
            EditedItem = item;
            UpdateBindings();
            var result = this.ShowDialog();
            return EditedItem;
        }

        private void UpdateBindings()
        {
            if (ListBox.DataSource == null) ListBox.DataSource = bindingSource1;

            if (EditedItem != null)
            {
                bindingSource1.DataSource = EditedItem;
            }

        }

        private void UpdateSelectedBindings()
        {
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();

            if (ListBox.SelectedItem != null && ListBox.SelectedItem is UpdateAssistant.Structure.ExtendedString)
            {
                textBox1.DataBindings.Add(new Binding("Text", (ListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String1"));
                textBox2.DataBindings.Add(new Binding("Text", (ListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String2"));
            }
        }

        private void UpdateSelectedButtons()
        {
            moveUpButton.Visible = false;
            moveDownButton.Visible = false;

            if (ListBox.SelectedItem != null)
            {
                removeButton.Enabled = true;
                moveUpButton.Enabled = true;
                moveDownButton.Enabled = true;

                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
                moveUpButton.Enabled = false;
                moveDownButton.Enabled = false;

                textBox1.Enabled = false;
                textBox2.Enabled = false;
            }
        }

        private void versionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedBindings();
            UpdateSelectedButtons();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            bindingSource1.Add(new UpdateAssistant.Structure.ExtendedString("New Site", "NULL"));
            UpdateBindings();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            bindingSource1.Remove(ListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString);
            UpdateBindings();
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {

        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {

        }

        private void textbox1_TextChanged(object sender, EventArgs e)
        {
            if (ListBox.SelectedItem != null && ListBox.SelectedIndex != -1 && EditedItem.Count > ListBox.SelectedIndex)
            {
                EditedItem[ListBox.SelectedIndex].String1 = textBox1.Text;
                ListBox.Refresh();
            }
        }

        private void textbox2_TextChanged(object sender, EventArgs e)
        {
            if (ListBox.SelectedItem != null && ListBox.SelectedIndex != -1 && EditedItem.Count > ListBox.SelectedIndex)
            {
                EditedItem[ListBox.SelectedIndex].String2 = textBox2.Text;
                ListBox.Refresh();
            }
        }

        private void SimpleListEditor_VisibleChanged(object sender, EventArgs e)
        {

        }
    }
}
