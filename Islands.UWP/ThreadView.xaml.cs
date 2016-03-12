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
    public sealed partial class ThreadView : UserControl
    {
        public Visibility threadIsHadTitle
        {
            get
            {
                if (threadTitle != "标题:" && threadTitle != "标题:无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility threadIsHadEmail
        {
            get
            {
                if (threadEmail != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility threadIsHadName
        {
            get
            {
                if (threadName != "名字:" && threadName != "名字:无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public string threadTitle
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "标题:" + thread.title;
                    default: return "";
                }
            }
        }
        public string threadEmail
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "email:" + thread.email;
                    default: return "";
                }
            }
        }
        public string threadName
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "名字:" + thread.name;
                    default: return "";
                }
            }
        }
        public string threadNo
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return thread.id;
                    default: return "";
                }
            }
        }

        public string threadCreateDate
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: return thread.now;
                    case IslandsCode.Koukuko:
                        DateTime dt = new DateTime(1970, 1, 1).ToLocalTime();
                        return dt.AddMilliseconds(Convert.ToDouble(thread.createdAt)).ToString("yyyy-MM-dd hh:mm:ss");
                    default: return "";
                }
            }
        }
        public string threadID
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: return thread.userid;
                    case IslandsCode.Koukuko: return thread.uid;
                    default: return "";
                }
            }
        }
        public string threadReplyCount
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return thread.replyCount;
                    default: return "";
                }
            }
        }
        public string threadThumb
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.A.PictureHost + "thumb/" + thread.img + thread.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.B.PictureHost + thread.img + "_t" + thread.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(thread.thumb))
                            return "";
                        return (Config.K.PictureHost + thread.thumb);
                    default: return "";
                }
            }
        }
        public string threadImage
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.A.PictureHost + thread.img + thread.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.B.PictureHost + thread.img + thread.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(thread.image))
                            return "";
                        return (Config.K.PictureHost + thread.image);
                    default: return "";
                }
            }
        }
        public string threadContent
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return thread.content;
                    default: return "";
                }
            }
        }

        Model.ThreadModel thread { get; set; }
        IslandsCode islandCode { get; set; }
        public ThreadView(Model.ThreadModel thread, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.thread = thread;
            this.islandCode = islandCode;
            if (!string.IsNullOrEmpty(this.threadThumb))
                imageBox.Source = new BitmapImage(new Uri(this.threadThumb));
            
        }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(threadThumb))
                imageBox.Source = new BitmapImage(new Uri("ms-appx:/Assets/luwei.jpg", UriKind.RelativeOrAbsolute));
        }
    }
}
