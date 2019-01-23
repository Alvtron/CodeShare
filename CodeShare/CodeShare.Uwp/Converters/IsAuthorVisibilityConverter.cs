using CodeShare.Model;
using CodeShare.Uwp.Services;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CodeShare.Uwp.Converters
{
    public class IsAuthorVisibilityConverter : IValueConverter
    {
        private const Visibility Visible = Visibility.Visible;
        private const Visibility Invisible = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is User user) return user.Uid == AuthService.CurrentUser.Uid ? Visible : Invisible;

            return value != null ? Visible : Invisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}