using Islands.UWP.Model;
using Islands.UWP.ViewModel;
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

    public class ItemToViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ItemViewModel viewModel = null;
            if (value is SendModel)
            {
                viewModel = new ItemViewModel(value as SendModel);
            }
            else
            {
                viewModel = new ItemViewModel()
                {
                    GlobalConfig = MainPage.Global,
                    BaseItem = value as BaseItemModel,
                };
                var t = value as ThreadModel;
                if (t != null) viewModel.ItemReplyCount = t.replyCount;
            }
            return (ItemViewModel)viewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
