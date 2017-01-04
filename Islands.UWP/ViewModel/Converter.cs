using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Islands.UWP.ViewModel
{
    public class ShowConverter : IValueConverter
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

    public class HideMenuConverter : IValueConverter
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

    public class HideIsNullOrWhiteSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = value as String;
            if (!string.IsNullOrWhiteSpace(result))
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

    public class ShowImageViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = value as String;
            if (!string.IsNullOrWhiteSpace(result))
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
                case DataTypes.Mark:
                    viewModel = new ItemViewModel()
                    {
                        GlobalConfig = MainPage.Global,
                        BaseItem = item as BaseItemModel,
                    };
                    var para = dataModel.Parameter as ItemParameter;
                    if (para != null)
                    {
                        viewModel.IsPo = para.IsPo;
                        viewModel.IsTextSelectionEnabled = para.IsTextSelectionEnabled;
                        viewModel.IsRef = para.IsRef;
                        viewModel.ParentList = para.ParentList as List<DataModel>;
                    }
                    break;
                case DataTypes.MyReply:
                    viewModel = new ItemViewModel(item as SendModel);
                    break;
            }
            if (viewModel != null) viewModel.DataType = type;
            return (ItemViewModel)viewModel;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    //public class IsShowRefBackgroundConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        bool? result = value as Nullable<bool>;
    //        if (result == true)
    //        {
    //            return Visibility.Visible;
    //        }
    //        return Visibility.Collapsed;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        return null;
    //    }
    //}
}
