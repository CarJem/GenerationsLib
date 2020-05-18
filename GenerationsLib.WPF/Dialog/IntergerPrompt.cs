using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GenerationsLib.WPF
{
	/// <summary>
	/// Interaction logic for TextPrompt2.xaml
	/// </summary>
	public partial class IntergerPrompt : Window
	{


		public IntergerPrompt(string label, string title, int defaultValue = 0)
		{
			InitializeComponent();
			this.SourceInitialized += WindowHelper.RemoveIcon;
			this.Loaded += new RoutedEventHandler(PromptDialog_Loaded);
			textLabel.Text = label;
			Title = title;
			textBox1.Value = defaultValue;
		}

		void PromptDialog_Loaded(object sender, RoutedEventArgs e)
		{

		}

		public static int ShowDialog(string title, string label, int defaultValue = 0)
		{
			IntergerPrompt inst = new IntergerPrompt(label, title, defaultValue);
			inst.ShowDialog();
			if (inst.DialogResult == true)
				return inst.ResponseVal;
			return defaultValue;
		}

		public int ResponseVal
		{
			get
			{
				return textBox1.Value ?? 0;
			}
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

	}
}
