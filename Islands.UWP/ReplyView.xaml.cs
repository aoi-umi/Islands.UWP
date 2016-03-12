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
    public sealed partial class ReplyView : UserControl
    {
        public Visibility replyIsHadTitle
        {
            get
            {
                if (replyTitle != "标题:" && replyTitle != "标题:无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility replyIsHadEmail
        {
            get
            {
                if (replyEmail != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility replyIsHadName
        {
            get
            {
                if (replyName != "名字:" && replyName != "名字:无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public string replyTitle
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "标题:" + reply.title;
                    default: return "";
                }
            }
        }
        public string replyEmail
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "email:" + reply.email;
                    default: return "";
                }
            }
        }
        public string replyName
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return "名字:" + reply.name;
                    default: return "";
                }
            }
        }
        public string replyNo
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return reply.id;
                    default: return "";
                }
            }
        }

        public string replyCreateDate
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: return reply.now;
                    case IslandsCode.Koukuko:
                        DateTime dt = new DateTime(1970, 1, 1).ToLocalTime();
                        return dt.AddMilliseconds(Convert.ToDouble(reply.createdAt)).ToString("yyyy-MM-dd hh:mm:ss");
                    default: return "";
                }
            }
        }
        public string replyID
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: return reply.userid;
                    case IslandsCode.Koukuko: return reply.uid;
                    default: return "";
                }
            }
        }

        public string replyThumb
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(reply.img))
                            return "";
                        return (Config.A.PictureHost + "thumb/" + reply.img + reply.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(reply.img))
                            return "";
                        return (Config.B.PictureHost + reply.img + "_t" + reply.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(reply.thumb))
                            return "";
                        return (Config.K.PictureHost + reply.thumb);
                    default: return "";
                }
            }
        }
        public string replyImage
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(reply.img))
                            return "";
                        return (Config.A.PictureHost + "image/" + reply.img + reply.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(reply.img))
                            return "";
                        return (Config.B.PictureHost + reply.img + reply.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(reply.image))
                            return "";
                        return (Config.K.PictureHost + reply.image);
                    default: return "";
                }
            }
        }
        public string replyContent
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return reply.content;
                    default: return "";
                }
            }
        }

        Model.ReplyModel reply {get; set; }
        IslandsCode islandCode { get; set; }
        public ReplyView(Model.ReplyModel reply, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.reply = reply;
            this.islandCode = islandCode;
            if (!string.IsNullOrEmpty(this.replyThumb))
                imageBox.Source = new BitmapImage(new Uri(this.replyThumb));
        }


        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(replyImage))
                imageBox.Source = new BitmapImage(new Uri("ms-appx:/Assets/luwei.jpg", UriKind.RelativeOrAbsolute));
        }
    }
}
