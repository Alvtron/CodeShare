using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CodeShare.Uwp.Converters
{
    public class InverseVisibilityConverter : IValueConverter
    {
        private const Visibility Visible = Visibility.Visible;
        private const Visibility Collapsed = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value != null ? Collapsed : Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return (visibility != Visible);
            }

            return true;
        }
    }
}
