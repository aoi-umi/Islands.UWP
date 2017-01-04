using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ImageView : BaseContentView
    {
        public ImageView()
        {
            InitializeComponent();
            SaveButton.Click += SaveButton_Click;
            image.ImageOpened += ImageView_ImageOpened;
            image.ImageFailed += ImageView_ImageFailed;
        }

        private string _ImageUrl { get; set; }
        public string ImageUrl
        {
            set
            {
                IsLoading = true;
                if(value != Config.FailedImageUri)
                    _ImageUrl = value;
                image.Width = Config.MaxImageWidth;
                image.Height = double.NaN;
                image.Source = string.IsNullOrEmpty(value) ? null : new BitmapImage(new Uri(value));
            }
            private get
            {
                return _ImageUrl;
            }
        }

        private void ImageView_ImageOpened(object sender, RoutedEventArgs e)
        {
            IsLoading = false;
        }

        private void ImageView_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ImageUrl = Config.FailedImageUri;
            IsLoading = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                Data.Message.ShowMessage("链接为空");
            }
            else
            {
                if (MainPage.Global.IsAskEachTime) Data.File.SaveFile(ImageUrl);
                else Data.File.SaveFileWithoutDialog(ImageUrl);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            image.Width = double.NaN;
            image.Height = double.NaN;
            ImageUrl = ImageUrl;
        }

        private void Image_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var flyout = this.Resources["ImageMenuFlyout"] as MenuFlyout;
            flyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
            //FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void MenuFlyoutCopyImage_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(ImageUrl)));
            Clipboard.SetContent(dataPackage);
        }

        ~ImageView()
        {
            SaveButton.Click -= SaveButton_Click;
            image.ImageOpened -= ImageView_ImageOpened;
            image.ImageFailed -= ImageView_ImageFailed;
        }
    }
}
