using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TranslationTool.ViewModels;

namespace TranslationTool.Converter
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public bool IsReverse { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Visible;
            if (value == null)
            {
                visibility = Visibility.Collapsed;
            }
            else if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                visibility = Visibility.Collapsed;
            }

            return visibility == Visibility.Visible ? (IsReverse ? Visibility.Collapsed : Visibility.Visible) : (IsReverse ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
