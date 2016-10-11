using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplyView : UserControl
    {

        public MyReplyView(Model.SendModel myReply, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.myReply = myReply;
            this.islandCode = islandCode;
            if (!string.IsNullOrEmpty(myReplyImage))
                Data.File.SetLocalImage(imageBox, myReplyImage);
        }

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

        public Model.SendModel myReply {get; set; }

        private IslandsCode islandCode { get; set; }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(myReplyImage))
                imageBox.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
        }
    }
}
