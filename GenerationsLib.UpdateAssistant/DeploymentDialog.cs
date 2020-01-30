using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerationsLib.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GenerationsLib.Gong.Shell;
using System.IO;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Web;

namespace GenerationsLib.UpdateAssistant
{
    public partial class DeploymentDialog : Form
    {
        public DeploymentDialog()
        {
            InitializeComponent();
            InitilizeTreeViewIcons();
            InitFileExplorer();
            InitWebBrowser();
            DialogDefaults();
        }
        private void DialogDefaults()
        {
            //this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private Process ExplorerHost { get; set; }

        public static UpdateAssistant EditedItem { get; set; }
        public void ShowConfigDialog(UpdateAssistant item)
        {
            EditedItem = item;
            RefreshTool();
        }

        private void SetupControls()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();

            this.Text = EditedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (publishHostsListBox.SelectedItem != null)
            {
                try
                {
                    string url = (publishHostsListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString).String2;
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateButtons()
        {
            if (publishHostsListBox.SelectedItem != null)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }

            if (downloadHostListBox.SelectedItem != null)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }


            if (SocialPost_listBox.SelectedItem != null)
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

        private void PraseMetadata()
        {
            richTextBox1.ResetText();
            richTextBox3.ResetText();

            groupBox6.Text = groupBox6.Tag.ToString();
            groupBox7.Text = groupBox7.Tag.ToString();

            if (versionListBox.SelectedItem != null)
            {
                try
                {
                    UpdateAssistant.Structure.Metadata meta = (versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata);
                    string changelog = meta.Details;
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(meta, Newtonsoft.Json.Formatting.Indented);

                    groupBox6.Text = groupBox6.Tag.ToString() + " - " + meta.Version;
                    groupBox7.Text = groupBox7.Tag.ToString() + " - " + meta.Version;

                    richTextBox1.Text = data;
                    richTextBox3.Text = changelog;
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
            groupBox5.Text = groupBox5.Tag.ToString();
            if (SocialPost_listBox.SelectedItem != null)
            {
                try
                {
                    UpdateAssistant.Structure.SiteForPostData meta = (SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData);
                    string data = meta.Notes;
                    richTextBox2.Text = data;
                    groupBox5.Text = groupBox5.Tag.ToString() + " - " + meta.Name;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (downloadHostListBox.SelectedItem != null)
            {
                try
                {
                    string url = (downloadHostListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString).String2;
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
            if (SocialPost_listBox.SelectedItem != null)
            {
                try
                {
                    string url = (SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData).URL;
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            string path = EditedItem.Path;
            EditedItem = null;
            EditedItem = new UpdateAssistant(path);
            RefreshTool();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void RefreshTool()
        {
            SetupControls();
            UpdateButtons();
            UpdateVersionMetadata();
            UpdateSocialPosts();
            UpdateDownloadPosts();
            UpdatePublishHosts();
            UpdatePublishScriptCodeBindings();
        }

        #region Version Metadata Editor

        private void UpdateVersionMetadata()
        {
            UpdateVerMetaSelectedButtons();
            UpdateVerMetaBindings();
        }

        private void UpdateVerMetaBindings(bool forced = false)
        {
            if (versionListBox.DataSource == null || forced)
            {
                if (versionListBox.DataSource != null) versionListBox.DataSource = null;
                versionListBox.DataSource = versionBindingSource;
            }

            if (EditedItem != null)
            {
                versionBindingSource.DataSource = EditedItem.Details.VersionMetadata;
            }

        }

        private void UpdateVerMetaSelectedBindings()
        {
            VerMeta_DownloadURLTextBox.DataBindings.Clear();
            VerMeta_ItemNameTextbox.DataBindings.Clear();
            VerMeta_DescriptionTextBox.DataBindings.Clear();

            if (versionListBox.SelectedItem != null && versionListBox.SelectedItem is UpdateAssistant.Structure.Metadata)
            {
                VerMeta_ItemNameTextbox.DataBindings.Add(new Binding("Text", (versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata), "Version"));
                VerMeta_DescriptionTextBox.DataBindings.Add(new Binding("Text", (versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata), "Details"));
                VerMeta_DownloadURLTextBox.DataBindings.Add(new Binding("Text", (versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata), "DownloadURL"));
            }

        }

        private void UpdateVerMetaSelectedButtons()
        {
            if (versionListBox.SelectedItem != null)
            {
                removeVerMetaButton.Enabled = true;
                moveUpVerMetaButton.Enabled = true;
                moveDownVerMetaButton.Enabled = true;

                VerMeta_DownloadURLTextBox.Enabled = true;
                VerMeta_ItemNameTextbox.Enabled = true;
                VerMeta_DescriptionTextBox.Enabled = true;
            }
            else
            {
                removeVerMetaButton.Enabled = false;
                moveUpVerMetaButton.Enabled = false;
                moveDownVerMetaButton.Enabled = false;

                VerMeta_DownloadURLTextBox.Enabled = false;
                VerMeta_ItemNameTextbox.Enabled = false;
                VerMeta_DescriptionTextBox.Enabled = false;
            }
        }

        private void versionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVerMetaSelectedBindings();
            UpdateVerMetaSelectedButtons();
            UpdateButtons();
        }

        private void addVerMetaButton_Click(object sender, EventArgs e)
        {
            var newItem = new UpdateAssistant.Structure.Metadata();
            newItem.Version = "NULL";
            versionBindingSource.Add(newItem);
            UpdateVerMetaBindings();
        }

        private void removeVerMetaButton_Click(object sender, EventArgs e)
        {
            versionBindingSource.Remove(versionListBox.SelectedItem as UpdateAssistant.Structure.Metadata);
            UpdateVerMetaBindings();
        }

        private void moveUpVerMetaButton_Click(object sender, EventArgs e)
        {
            int index = versionListBox.SelectedIndex;
            EditedItem.Details.VersionMetadata.Move(index, index - 1);
            UpdateVerMetaBindings(true);
            versionListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(versionListBox.Items.Count, index, index - 1);
        }

        private void moveDownVerMetaButton_Click(object sender, EventArgs e)
        {
            int index = versionListBox.SelectedIndex;
            EditedItem.Details.VersionMetadata.Move(index, index + 1);
            UpdateVerMetaBindings(true);
            versionListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(versionListBox.Items.Count, index, index + 1);
        }

        #endregion

        #region Social Posts Editor

        private void UpdateSocialPosts()
        {
            UpdateSocialPostBindings();
            UpdateSelectedSocialPostButtons();
        }

        private void UpdateSocialPostBindings(bool forced = false)
        {
            if (SocialPost_listBox.DataSource == null || forced)
            {
                if (SocialPost_listBox.DataSource != null) SocialPost_listBox.DataSource = null;
                SocialPost_listBox.DataSource = SocialPostBindingSource;
            }

            if (EditedItem != null)
            {
                SocialPostBindingSource.DataSource = EditedItem.Details.PlacesToPost;
            }

        }

        private void UpdateSelectedSocialPostBindings()
        {
            SocialPost_downloadURLTextBox.DataBindings.Clear();
            SocialPost_itemNameTextbox.DataBindings.Clear();
            SocialPost_descriptionTextBox.DataBindings.Clear();

            if (SocialPost_listBox.SelectedItem != null && SocialPost_listBox.SelectedItem is UpdateAssistant.Structure.SiteForPostData)
            {
                SocialPost_itemNameTextbox.DataBindings.Add(new Binding("Text", (SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "Name"));
                SocialPost_descriptionTextBox.DataBindings.Add(new Binding("Text", (SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "Notes"));
                SocialPost_downloadURLTextBox.DataBindings.Add(new Binding("Text", (SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData), "URL"));
            }

        }

        private void UpdateSelectedSocialPostButtons()
        {
            if (SocialPost_listBox.SelectedItem != null)
            {
                removeSocialPostButton.Enabled = true;
                moveSocialPostUpButton.Enabled = true;
                moveSocialPostDownButton.Enabled = true;

                SocialPost_downloadURLTextBox.Enabled = true;
                SocialPost_itemNameTextbox.Enabled = true;
                SocialPost_descriptionTextBox.Enabled = true;
            }
            else
            {
                removeSocialPostButton.Enabled = false;
                moveSocialPostUpButton.Enabled = false;
                moveSocialPostDownButton.Enabled = false;

                SocialPost_downloadURLTextBox.Enabled = false;
                SocialPost_itemNameTextbox.Enabled = false;
                SocialPost_descriptionTextBox.Enabled = false;
            }
        }

        private void socialListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
            UpdateSelectedSocialPostBindings();
            UpdateSelectedSocialPostButtons();
        }

        private void addSocialPostButton_Click(object sender, EventArgs e)
        {
            var newItem = new UpdateAssistant.Structure.SiteForPostData();
            newItem.Name = "NULL";
            SocialPostBindingSource.Add(newItem);
            UpdateSocialPostBindings();
        }

        private void removeSocialPostButton_Click(object sender, EventArgs e)
        {
            SocialPostBindingSource.Remove(SocialPost_listBox.SelectedItem as UpdateAssistant.Structure.SiteForPostData);
            UpdateSocialPostBindings();
        }

        private void moveSocialPostUpButton_Click(object sender, EventArgs e)
        {
            int index = SocialPost_listBox.SelectedIndex;
            EditedItem.Details.PlacesToPost.Move(index, index - 1);
            UpdateSocialPostBindings(true);
            SocialPost_listBox.SelectedIndex = ListHelpers.MoveSelectedIndex(SocialPost_listBox.Items.Count, index, index - 1);
        }

        private void moveSocialPostDownButton_Click(object sender, EventArgs e)
        {
            int index = SocialPost_listBox.SelectedIndex;
            EditedItem.Details.PlacesToPost.Move(index, index + 1);
            UpdateSocialPostBindings(true);
            SocialPost_listBox.SelectedIndex = ListHelpers.MoveSelectedIndex(SocialPost_listBox.Items.Count, index, index + 1);
        }

        private void SocialPost_ItemNameTextbox_TextChanged(object sender, EventArgs e)
        {
            SocialPost_listBox.Refresh();
        }

        #endregion

        #region Download Hosts Editor

        private void UpdateDownloadPosts()
        {
            UpdateDownloadHostsBindings();
            UpdateSelectedDownloadHostsButtons();
        }
        private void UpdateDownloadHostsBindings(bool forced = false)
        {
            if (downloadHostListBox.DataSource == null || forced)
            {
                if (downloadHostListBox.DataSource != null) downloadHostListBox.DataSource = null;
                downloadHostListBox.DataSource = downloadHostsBindingSource;
            }

            if (EditedItem != null)
            {
                downloadHostsBindingSource.DataSource = EditedItem.Details.DownloadHosts;
            }

        }

        private void UpdateSelectedDownloadHostsBindings()
        {
            downloadHostsTextBox1.DataBindings.Clear();
            downloadHostsTextBox2.DataBindings.Clear();

            if (downloadHostListBox.SelectedItem != null && downloadHostListBox.SelectedItem is UpdateAssistant.Structure.ExtendedString)
            {
                downloadHostsTextBox1.DataBindings.Add(new Binding("Text", (downloadHostListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String1"));
                downloadHostsTextBox2.DataBindings.Add(new Binding("Text", (downloadHostListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String2"));
            }
        }

        private void UpdateSelectedDownloadHostsButtons()
        {
            if (downloadHostListBox.SelectedItem != null)
            {
                removeDownloadHostsButton.Enabled = true;
                moveDownloadHostsUpButton.Enabled = true;
                moveDownloadHostsDownButton.Enabled = true;

                downloadHostsTextBox1.Enabled = true;
                downloadHostsTextBox2.Enabled = true;
            }
            else
            {
                removeDownloadHostsButton.Enabled = false;
                moveDownloadHostsUpButton.Enabled = false;
                moveDownloadHostsDownButton.Enabled = false;

                downloadHostsTextBox1.Enabled = false;
                downloadHostsTextBox2.Enabled = false;
            }
        }

        private void DownloadHostListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
            UpdateSelectedDownloadHostsBindings();
            UpdateSelectedDownloadHostsButtons();
        }

        private void addDownloadHostButton_Click(object sender, EventArgs e)
        {
            downloadHostsBindingSource.Add(new UpdateAssistant.Structure.ExtendedString("New Site", "NULL"));
            UpdateDownloadHostsBindings();
        }

        private void removeDownloadHostButton_Click(object sender, EventArgs e)
        {
            downloadHostsBindingSource.Remove(downloadHostListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString);
            UpdateDownloadHostsBindings();
        }

        private void moveDownloadHostUpButton_Click(object sender, EventArgs e)
        {
            int index = downloadHostListBox.SelectedIndex;
            EditedItem.Details.DownloadHosts.Move(index, index - 1);
            UpdateDownloadHostsBindings(true);
            downloadHostListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(downloadHostListBox.Items.Count, index, index - 1);
        }

        private void moveDownloadHostDownButton_Click(object sender, EventArgs e)
        {
            int index = downloadHostListBox.SelectedIndex;
            EditedItem.Details.DownloadHosts.Move(index, index + 1);
            UpdateDownloadHostsBindings(true);
            downloadHostListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(downloadHostListBox.Items.Count, index, index + 1);
        }

        private void DownloadHost_textbox1_TextChanged(object sender, EventArgs e)
        {
            if (downloadHostListBox.SelectedItem != null && downloadHostListBox.SelectedIndex != -1 && EditedItem.Details.DownloadHosts.Count > downloadHostListBox.SelectedIndex)
            {
                EditedItem.Details.DownloadHosts[downloadHostListBox.SelectedIndex].String1 = downloadHostsTextBox1.Text;
                downloadHostListBox.Refresh();
            }
        }

        private void DownloadHost_textbox2_TextChanged(object sender, EventArgs e)
        {
            if (downloadHostListBox.SelectedItem != null && downloadHostListBox.SelectedIndex != -1 && EditedItem.Details.DownloadHosts.Count > downloadHostListBox.SelectedIndex)
            {
                EditedItem.Details.DownloadHosts[downloadHostListBox.SelectedIndex].String2 = downloadHostsTextBox2.Text;
                downloadHostListBox.Refresh();
            }
        }

        #endregion

        #region Publish Hosts Editor 

        private void UpdatePublishHosts()
        {
            UpdatePublishHostsBindings();
            UpdateSelectedPublishHostsButtons();
        }

        private void UpdatePublishHostsBindings(bool forced = false)
        {
            if (publishHostsListBox.DataSource == null || forced)
            {
                if (publishHostsListBox.DataSource != null) publishHostsListBox.DataSource = null;
                publishHostsListBox.DataSource = publishHostsBindingSource;
            }

            if (EditedItem != null)
            {
                publishHostsBindingSource.DataSource = EditedItem.Details.SitesToPublishTo;
            }

        }

        private void UpdateSelectedPublishHostsBindings()
        {
            publishHosts_textBox1.DataBindings.Clear();
            publishHosts_textBox2.DataBindings.Clear();

            if (publishHostsListBox.SelectedItem != null && publishHostsListBox.SelectedItem is UpdateAssistant.Structure.ExtendedString)
            {
                publishHosts_textBox1.DataBindings.Add(new Binding("Text", (publishHostsListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String1"));
                publishHosts_textBox2.DataBindings.Add(new Binding("Text", (publishHostsListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString), "String2"));
            }
        }

        private void UpdateSelectedPublishHostsButtons()
        {
            if (publishHostsListBox.SelectedItem != null)
            {
                publishHosts_removeButton.Enabled = true;
                publishHosts_moveUpButton.Enabled = true;
                publishHosts_moveDownButton.Enabled = true;

                publishHosts_textBox1.Enabled = true;
                publishHosts_textBox2.Enabled = true;
            }
            else
            {
                publishHosts_removeButton.Enabled = false;
                publishHosts_moveUpButton.Enabled = false;
                publishHosts_moveDownButton.Enabled = false;

                publishHosts_textBox1.Enabled = false;
                publishHosts_textBox2.Enabled = false;
            }
        }

        private void PublishHostListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
            UpdateSelectedPublishHostsBindings();
            UpdateSelectedPublishHostsButtons();
        }

        private void addPublishHostButton_Click(object sender, EventArgs e)
        {
            publishHostsBindingSource.Add(new UpdateAssistant.Structure.ExtendedString("New Site", "NULL"));
            UpdatePublishHostsBindings();
        }

        private void removePublishHostButton_Click(object sender, EventArgs e)
        {
            publishHostsBindingSource.Remove(publishHostsListBox.SelectedItem as UpdateAssistant.Structure.ExtendedString);
            UpdatePublishHostsBindings();
        }

        private void movePublishHostUpButton_Click(object sender, EventArgs e)
        {
            int index = publishHostsListBox.SelectedIndex;
            EditedItem.Details.SitesToPublishTo.Move(index, index - 1);
            UpdatePublishHostsBindings(true);
            publishHostsListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(publishHostsListBox.Items.Count, index, index - 1);
        }

        private void movePublishHostDownButton_Click(object sender, EventArgs e)
        {
            int index = publishHostsListBox.SelectedIndex;
            EditedItem.Details.SitesToPublishTo.Move(index, index + 1);
            UpdatePublishHostsBindings(true);
            publishHostsListBox.SelectedIndex = ListHelpers.MoveSelectedIndex(publishHostsListBox.Items.Count, index, index + 1);
        }

        private void PublishHost_textbox1_TextChanged(object sender, EventArgs e)
        {
            if (publishHostsListBox.SelectedItem != null && publishHostsListBox.SelectedIndex != -1 && EditedItem.Details.SitesToPublishTo.Count > publishHostsListBox.SelectedIndex)
            {
                EditedItem.Details.SitesToPublishTo[publishHostsListBox.SelectedIndex].String1 = publishHosts_textBox1.Text;
                publishHostsListBox.Refresh();
            }
        }

        private void PublishHost_textbox2_TextChanged(object sender, EventArgs e)
        {
            if (publishHostsListBox.SelectedItem != null && publishHostsListBox.SelectedIndex != -1 && EditedItem.Details.SitesToPublishTo.Count > publishHostsListBox.SelectedIndex)
            {
                EditedItem.Details.SitesToPublishTo[publishHostsListBox.SelectedIndex].String2 = publishHosts_textBox2.Text;
                publishHostsListBox.Refresh();
            }
        }

        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {
            EditedItem.Save();
            RefreshTool();
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            string name = Essy.Tools.InputBox.InputBox.ShowInputBox("Item Name:", EditedItem.Details.Name, false);
            EditedItem.Details.Name = name;
            RefreshTool();
        }

        private void setIDButton_Click(object sender, EventArgs e)
        {
            string name = Essy.Tools.InputBox.InputBox.ShowInputBox("Item ID:", EditedItem.Details.ID, false);
            EditedItem.Details.ID = name;
            RefreshTool();
        }

        private void toggleInfoPaneButton_Click(object sender, EventArgs e)
        {
            if (!splitContainer2.Panel2Collapsed) splitContainer2.Panel2Collapsed = true;
            else splitContainer2.Panel2Collapsed = false;
        }

        #region File Explorer

        private ShellComboBox File_ShellComboBox { get; set; }
        private ShellView File_ShellView { get; set; }
        private ShellTreeView File_ShellTreeView { get; set; }

        private void InitFileExplorer()
        {
            //Create Explorer Controls
            File_ShellComboBox = new ShellComboBox();
            File_ShellView = new ShellView();
            File_ShellTreeView = new ShellTreeView();

            //Set Shell View of Other Controls
            File_ShellComboBox.ShellView = File_ShellView;
            File_ShellTreeView.ShellView = File_ShellView;

            //Combo Box Settings
            File_ShellComboBox.ShowFileSystemPath = true;

            //Shell View Settings
            File_ShellView.View = ShellViewStyle.Details;
            File_ShellView.Navigated += File_ShellView_Navigated;
            File_ShellView.Navigating += File_ShellView_Navigating;

            //Tree View Settings
            File_ShellTreeView.HotTracking = true;

            //Set all to Dock in Place
            File_ShellView.Dock = DockStyle.Fill;
            File_ShellComboBox.Dock = DockStyle.Fill;
            File_ShellTreeView.Dock = DockStyle.Fill;

            //Add the Controls
            fileComboBoxPanel.Controls.Add(File_ShellComboBox);
            fileTreeViewPanel.Controls.Add(File_ShellTreeView);
            fileViewPanel.Controls.Add(File_ShellView);
        }

        private void File_ShellView_Navigating(object sender, NavigatingEventArgs e)
        {
            UpdateFileBackForwardButtons();
        }

        private void File_ShellView_Navigated(object sender, EventArgs e)
        {
            UpdateFileBackForwardButtons();
        }

        private void UpdateFileBackForwardButtons()
        {
            if (File_ShellView != null)
            {
                if (File_ShellView.CanNavigateForward) fileForwardButton.Enabled = true;
                else fileForwardButton.Enabled = false;

                if (File_ShellView.CanNavigateBack) fileBackButton.Enabled = true;
                else fileBackButton.Enabled = false;
            }

        }

        private void InitilizeTreeViewIcons()
        {
            ImageList MyImages = new ImageList();
            MyImages.ImageSize = new Size(16, 16);

            MyImages.Images.Add(DefaultIcons.FolderLarge);

            quickAccessTreeView.ImageList = MyImages;
        }

        private void RefreshQuickAccessEntries()
        {
            quickAccessTreeView.Nodes.Clear();
            foreach (var entry in EditedItem.Details.DownloadHosts)
            {
                try
                {
                    string path = entry.String2.Replace("\"", "");
                    if (Directory.Exists(path + "\\"))
                    {
                        TreeNode node = new TreeNode();
                        node.Text = entry.String1;
                        node.Tag = path;
                        node.ImageIndex = 0;
                        quickAccessTreeView.Nodes.Add(node);
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid Entry");
                }

            }
        }

        private void quickAccessTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                File_ShellView.Navigate(e.Node.Tag.ToString());
            }
        }
        private void showfileQuickAccessButton_Click(object sender, EventArgs e)
        {
            fileExplorerContainer.Panel1Collapsed = !fileExplorerContainer.Panel1Collapsed;
        }

        private void fileForwardButton_Click(object sender, EventArgs e)
        {
            File_ShellView.NavigateForward();
        }

        private void fileBackButton_Click(object sender, EventArgs e)
        {
            File_ShellView.NavigateBack();
        }

        #endregion

        #region Web Browser

        private ChromiumWebBrowser ChromiumWebBrowser { get; set; }

        #region Chromium Context Menu

        internal class MenuHandler : IContextMenuHandler
        {
            private const int Copy = 26503;
            private string LastUnfilteredLinkUrl { get; set; }

            void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
            {
                //Add new custom menu items
                model.AddItem((CefMenuCommand)Copy, "Copy Link Address");
                SetupCopyLinkAddress(browserControl, browser, frame, parameters, model);
            }

            void SetupCopyLinkAddress(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
            {
                string linkURL = parameters.UnfilteredLinkUrl;

                if (linkURL != null && linkURL != "")
                {
                    model.SetEnabled((CefMenuCommand)Copy, true);
                    LastUnfilteredLinkUrl = parameters.UnfilteredLinkUrl;
                }
                else
                {
                    model.SetEnabled((CefMenuCommand)Copy, false);
                    LastUnfilteredLinkUrl = null;
                }
            }

            bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
            {
                if ((int)commandId == Copy)
                {
                    if (LastUnfilteredLinkUrl != null) Clipboard.SetText(LastUnfilteredLinkUrl);
                }
                return false;
            }

            void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
            {

            }

            bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
            {
                return false;
            }
        }

        #endregion

        private void InitWebBrowser()
        {
            // Create a browser component
            ChromiumWebBrowser = new ChromiumWebBrowser("http://google.com");
            ChromiumWebBrowser.BrowserSettings.ApplicationCache = CefSharp.CefState.Enabled;
            // Add it to the form and fill it to the form window.
            chromeHostPanel.Controls.Add(ChromiumWebBrowser);
            ChromiumWebBrowser.Dock = DockStyle.Fill;
            ChromiumWebBrowser.AddressChanged += ChromiumWebBrowser_AddressChanged;
            ChromiumWebBrowser.LoadingStateChanged += ChromiumWebBrowser_LoadingStateChanged;
            ChromiumWebBrowser.MenuHandler = new MenuHandler();
        }

        private void ChromiumWebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            UpdateBackwardsAndForwardsButtons();
        }

        private void ChromiumWebBrowser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            chromeAddressBar.Invoke((MethodInvoker)(() =>
            {
                chromeAddressBar.Text = ChromiumWebBrowser.Address;
            }));
            UpdateBackwardsAndForwardsButtons();
        }

        private void UpdateBackwardsAndForwardsButtons()
        {
            backwardsButton.Invoke((MethodInvoker)(() =>
            {
                if (ChromiumWebBrowser.CanGoBack) backwardsButton.Enabled = true;
                else backwardsButton.Enabled = false;
            }));

            forwardsButton.Invoke((MethodInvoker)(() =>
            {
                if (ChromiumWebBrowser.CanGoForward) forwardsButton.Enabled = true;
                else forwardsButton.Enabled = false;
            }));
        }

        private void backwardsButton_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser.Back();
        }

        private void forwardsButton_Click(object sender, EventArgs e)
        {
            ChromiumWebBrowser.Forward();
        }

        private void RefreshBookmarkEntries()
        {
            bookmarkTreeView.Nodes.Clear();
            RefreshDownloadHostsEntries();
            RefreshPublishingEntries();
            RefreshSocialEntries();


            void RefreshDownloadHostsEntries()
            {
                TreeNode downloadHosts = new TreeNode("Download Hosts");
                foreach (var entry in EditedItem.Details.DownloadHosts)
                {
                    try
                    {
                        string path = entry.String2.Replace("\"", "");
                        if (WebHelpers.isURLValid(path))
                        {
                            TreeNode node = new TreeNode();
                            node.Text = entry.String1;
                            node.Tag = path;
                            downloadHosts.Nodes.Add(node);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Entry");
                    }

                }
                bookmarkTreeView.Nodes.Add(downloadHosts);
            }

            void RefreshPublishingEntries()
            {
                TreeNode downloadHosts = new TreeNode("Publishing Hosts");
                foreach (var entry in EditedItem.Details.SitesToPublishTo)
                {
                    try
                    {
                        string path = entry.String2.Replace("\"", "");
                        if (WebHelpers.isURLValid(path))
                        {
                            TreeNode node = new TreeNode();
                            node.Text = entry.String1;
                            node.Tag = path;
                            downloadHosts.Nodes.Add(node);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Entry");
                    }

                }
                bookmarkTreeView.Nodes.Add(downloadHosts);
            }

            void RefreshSocialEntries()
            {
                TreeNode downloadHosts = new TreeNode("Social Sites");
                foreach (var entry in EditedItem.Details.PlacesToPost)
                {
                    try
                    {
                        string path = entry.URL.Replace("\"", "");
                        if (WebHelpers.isURLValid(path))
                        {
                            TreeNode node = new TreeNode();
                            node.Text = entry.Name;
                            node.Tag = path;
                            downloadHosts.Nodes.Add(node);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Entry");
                    }

                }
                bookmarkTreeView.Nodes.Add(downloadHosts);
            }
        }

        private void bookmarkTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                ChromiumWebBrowser.Load(e.Node.Tag.ToString());
            }
        }

        private void showQuickLinksButton_Click(object sender, EventArgs e)
        {
            splitContainer4.Panel1Collapsed = !splitContainer4.Panel1Collapsed;
        }

        private void chromeAddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChromiumWebBrowser.Load(chromeAddressBar.Text);
            }
        }

        #endregion

        #region Publish Script Code Editor

        private void UpdatePublishScriptCodeBindings()
        {
            publishScriptCodeTextbox.DataBindings.Clear();
            publishScriptCodeTextbox.DataBindings.Add(new Binding("Text", (EditedItem.Details), "PublishScriptCode"));
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                RefreshQuickAccessEntries();
            }
            else if (tabControl1.SelectedTab == tabPage8)
            {
                RefreshBookmarkEntries();
            }
        }


        private void DeploymentDialog_SizeChanged(object sender, EventArgs e)
        {

        }

        private void chromeHostPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
