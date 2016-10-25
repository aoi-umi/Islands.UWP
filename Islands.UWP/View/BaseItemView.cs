using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Islands.UWP
{
    public class BaseItemView : ContentControl
    {
        public BaseItemView()
        {
            this.DefaultStyleKey = typeof(BaseItemView);
        }

        public Visibility IsHadTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(ItemTitle) && ItemTitle != "标题:" && ItemTitle != "无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility IsHadEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(ItemEmail) && ItemEmail != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility IsHadName
        {
            get
            {
                if (!string.IsNullOrEmpty(ItemName) && ItemName != "名字:" && ItemName != "无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public string ItemTitle { get; set; }
        public string ItemEmail { get; set; }
        public string ItemName { get; set; }
        public string ItemNo { get; set; }
        public string ItemCreateDate { get; set; }
        public string ItemMsg { get; set; }
        public string ItemUid { get; set; }
        public string ItemReplyCount { get; set; }
        public string ItemThumb { get; set; }
        public string ItemImage { get; set; }
        public string ItemContent { get; set; }

        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
    }
}
