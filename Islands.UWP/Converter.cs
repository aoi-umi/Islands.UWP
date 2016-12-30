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

    public class IsHideMenuConverter : IValueConverter
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
            var dataModel = value as DataModel;
            var type = dataModel.DataType;
            var item = dataModel.Data;
            ItemViewModel viewModel = null;
            switch (type)
            {
                case DataTypes.Thread:
                case DataTypes.Reply:
                    viewModel = new ItemViewModel()
                    {
                        GlobalConfig = MainPage.Global,
                        BaseItem = item as BaseItemModel,
                    };
                    if (type == DataTypes.Thread) viewModel.ItemReplyCount = (item as ThreadModel).replyCount;
                    var para = dataModel.Parameter as ItemParameter;
                    if (para != null)
                    {
                        viewModel.IsPo = para.IsPo;
                        viewModel.IsTextSelectionEnabled = para.IsTextSelectionEnabled;
                    }
                    break;
                case DataTypes.MyReply:
                    viewModel = new ItemViewModel(item as SendModel);
                    break;
            }
            return (ItemViewModel)viewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
