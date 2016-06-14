using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Islands.UWP
{
    public class IsShowMenuButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? result = value as Nullable<bool>;
            if (result == true)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class IsShowMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? result = value as Nullable<bool>;
            if (result == true)
            {
                return (double)0;
            }
            return (double)35;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
