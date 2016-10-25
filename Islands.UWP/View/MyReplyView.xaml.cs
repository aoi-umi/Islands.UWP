﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplyView : BaseItemView
    {

        public MyReplyView(Model.SendModel myReply, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.myReply = myReply;
            this.islandCode = islandCode;            
            ItemTitle = string.IsNullOrEmpty(myReply.sendTitle) ? "" : "标题:" + myReply.sendTitle;
            ItemEmail = string.IsNullOrEmpty(myReply.sendEmail) ? "" : "email:" + myReply.sendEmail;
            ItemName = string.IsNullOrEmpty(myReply.sendName) ? "" : "名字:" + myReply.sendName;
            ItemNo = myReply.ThreadId;
            ItemCreateDate = myReply.sendDateTime;
            ItemMsg = (myReply.isMain ? "新串：" : "回复：") + myReply.sendId; 
            ItemImage = myReply.sendImage;
            ItemContent = myReply.sendContent;

            if (!string.IsNullOrEmpty(ItemImage))
                Data.File.SetLocalImage(imageBox, ItemImage);
        }

        public Model.SendModel myReply {get; set; }

        private IslandsCode islandCode { get; set; }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemImage))
                imageBox.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
        }

        private void ImageBox_ImageOpened(object sender, RoutedEventArgs e)
        {
            var bitmap = imageBox.Source as BitmapImage;
            if (bitmap != null && bitmap.PixelWidth < Config.MaxImageWidth && bitmap.PixelHeight < Config.MaxImageHeight) imageBox.Stretch = Stretch.None;
        }
    }
}
