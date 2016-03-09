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
        public string ReplyID { get { return reply.No; } }
        Model.ReplyModel reply;
        public ReplyView(Model.ReplyModel reply)
        {
            this.InitializeComponent();
            this.reply = reply;
            if (!string.IsNullOrEmpty(this.reply.Image))
                imageBox.Source = new BitmapImage(new Uri(this.reply.Image));
        }


        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(reply.Image))
                imageBox.Source = new BitmapImage(new Uri("ms-appx:/Assets/luwei.jpg", UriKind.RelativeOrAbsolute));
        }
    }
}
