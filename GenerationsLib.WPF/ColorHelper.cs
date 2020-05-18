using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using System.Windows.Controls;
using System.Windows.Media;

namespace GenerationsLib.WPF
{
    public static class ColorExt
    {
        public static SWMColor ToSWMColor(this SDColor color) => SWMColor.FromArgb(color.A, color.R, color.G, color.B);
        public static SDColor ToSDColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);

        private static SDColor GetResource(Control Source, string ResourceName)
        {
            try
            {
                var c = (SolidColorBrush)Source.FindResource(ResourceName);
                if (c != null) return SDColor.FromArgb(c.Color.A, c.Color.R, c.Color.G, c.Color.B);
                else return SDColor.Empty;
            }
            catch
            {
                return SDColor.Empty;
            }
        }

        public static SolidColorBrush GetSCBResource(Control Source, string ResourceName)
        {
            try
            {
                var c = (SolidColorBrush)Source.FindResource(ResourceName);
                if (c != null) return c;
                else return new SolidColorBrush();
            }
            catch
            {
                return new SolidColorBrush();
            }
        }
    }
}