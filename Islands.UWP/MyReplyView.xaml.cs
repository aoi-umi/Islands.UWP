using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplyView : UserControl
    {
        public Visibility myReplyIsHadTitle
        {
            get
            {
                if (myReplyTitle != "标题:" && myReplyTitle != "标题:无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility myReplyIsHadEmail
        {
            get
            {
                if (myReplyEmail != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility myReplyIsHadName
        {
            get
            {
                if (myReplyName != "名字:" && myReplyName != "名字:无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public string myReplyTitle
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "标题:" + myReply.sendTitle;
                    default: return "";
                }
            }
        }
        public string myReplyEmail
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "email:" + myReply.sendEmail;
                    default: return "";
                }
            }
        }
        public string myReplyName
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "名字:" + myReply.sendName;
                    default: return "";
                }
            }
        }
        public string myReplyNo
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return myReply.ThreadId;
                    default: return "";
                }
            }
        }

        public string myReplyCreateDate
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: 
                    case IslandsCode.Koukuko: return myReply.sendDateTime;
                    default: return "";
                }
            }
        }
        public string myReplyMsg
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: 
                    case IslandsCode.Koukuko: return (myReply.isMain ? "新串：" : "回复：") + myReply.sendId;
                    default: return "";
                }
            }
        }
        
        public string myReplyImage
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return myReply.sendImage;
                    default: return "";
                }
            }
        }
        public string myReplyContent
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return myReply.sendContent;
                    default: return "";
                }
            }
        }

        public bool IsCheckboxDisplay
        {
            set
            {
                if (value) IsSelectedBox.Visibility = Visibility.Visible;
                else {
                    IsSelectedBox.Visibility = Visibility.Collapsed;
                    IsSelected = false;
                    Background = null;
                }
            }
            get { return IsSelectedBox.Visibility == Visibility.Visible ? true : false; }
        }
        public bool IsSelected { set { IsSelectedBox.IsChecked = value; } get { return (bool)IsSelectedBox.IsChecked; } }

        public Model.SendModel myReply {get; set; }

        IslandsCode islandCode { get; set; }

        public MyReplyView(Model.SendModel myReply, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.myReply = myReply;
            this.islandCode = islandCode;
            if(!string.IsNullOrEmpty(myReplyImage))
                Data.File.SetLocalImage(imageBox, myReplyImage);
        }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(myReplyImage))
                imageBox.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
        }
    }
}
