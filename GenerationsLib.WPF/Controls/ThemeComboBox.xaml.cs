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
		public event EventHandler<Skin> ThemeChanged;

		public Skin SelectedSkin
		{
			get
			{
				UpdateVisual();
				return GetSkinFromSelectedIndex();
			}
			set
			{
				SetSkinFromSelectedIndex(value);
				UpdateVisual();
			}
		}

		private Brush NormalBorderBrush { get; set; }

        public ThemeComboBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			if (SkinResourceDictionary.CurrentTheme != GetSkinFromSelectedIndex())
			{
				if (this.ThemeChanged != null) this.ThemeChanged.Invoke(this, GetSkinFromSelectedIndex());
			}
		}

		private void SetSkinFromSelectedIndex(Skin newSkin)
		{
			switch (newSkin)
			{
				case Skin.Light:
					UserThemeComboBox.SelectedIndex = 0;
					break;
				case Skin.Dark:
					UserThemeComboBox.SelectedIndex = 1;
					break;
				case Skin.Beta:
					UserThemeComboBox.SelectedIndex = 2;
					break;
				case Skin.Shard:
					UserThemeComboBox.SelectedIndex = 3;
					break;
				case Skin.CarJem:
					UserThemeComboBox.SelectedIndex = 4;
					break;
				case Skin.Gamma:
					UserThemeComboBox.SelectedIndex = 5;
					break;
				case Skin.Sparks:
					UserThemeComboBox.SelectedIndex = 6;
					break;
				default:
					UserThemeComboBox.SelectedIndex = 0;
					break;
			}
		}

		private Skin GetSkinFromSelectedIndex()
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

		public void UpdateVisual()
		{
			if (SkinResourceDictionary.CurrentTheme != GetSkinFromSelectedIndex()) UserThemeComboBox.BorderBrush = Brushes.Red;
			else UserThemeComboBox.ClearValue(ComboBox.BorderBrushProperty);
		}

		private void UserThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateVisual();
		}
	}
}
