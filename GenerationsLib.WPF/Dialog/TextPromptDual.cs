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
	public partial class TextPromptDual : Window
	{


		private TextPromptDual(string label, string label2, string title, string defaultValue = "", string defaultValue2 = "")
		{
			InitializeComponent();
			this.SourceInitialized += WindowHelper.RemoveIcon;
			this.Loaded += new RoutedEventHandler(PromptDialog_Loaded);
			textLabel.Text = label;
			textLabel2.Text = label2;
			Title = title;
			textBox1.Text = defaultValue;
			textBox2.Text = defaultValue2;
		}

		void PromptDialog_Loaded(object sender, RoutedEventArgs e)
		{

		}

		public static Tuple<string, string> ShowDialog(string title, string label, string label2, string defaultValue = "", string defaultValue2 = "")
		{
			TextPromptDual inst = new TextPromptDual(label, label2, title, defaultValue, defaultValue2);
			inst.ShowDialog();
			if (inst.DialogResult == true)
				return new Tuple<string, string>(inst.ResponseTextA, inst.ResponseTextB);
			return new Tuple<string, string>(defaultValue, defaultValue2);
		}

		public string ResponseTextA
		{
			get
			{
				return textBox1.Text;
			}
		}

		public string ResponseTextB
		{
			get
			{
				return textBox2.Text;
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
