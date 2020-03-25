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
using System.Windows.Navigation;
using System.Windows.Shapes;

using GenerationsLib.WPF.Themes;

namespace GenerationsLib.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ThemeComboBox.xaml
    /// </summary>
    public partial class ThemeComboBox : UserControl
    {
		public EventHandler<Skin> ThemeChanged;
        public ThemeComboBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			if (SkinResourceDictionary.CurrentTheme != ApplySkinFromSelectedIndex())
			{
				if (this.ThemeChanged != null) this.ThemeChanged.Invoke(this, ApplySkinFromSelectedIndex());
			}
		}

      
		private Skin ApplySkinFromSelectedIndex()
		{
			switch (UserThemeComboBox.SelectedIndex)
			{
				case 0:
					return Skin.Light;
				case 1:
					return Skin.Dark;
				case 2:
					return Skin.Beta;
				case 3:
					return Skin.Shard;
				case 4:
					return Skin.CarJem;
				case 5:
					return Skin.Gamma;
				case 6:
					return Skin.Sparks;
				default:
					return Skin.Light;
			}
		}
	}
}
