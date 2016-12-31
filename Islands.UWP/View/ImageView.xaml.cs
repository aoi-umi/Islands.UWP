using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
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

        private string _ImgageUrl { get; set; }
        public string ImageUrl
        {
            set
            {
                IsLoading = true;
                if(value != Config.FailedImageUri)
                    _ImgageUrl = value;
                image.Width = Config.MaxImageWidth;
                image.Height = double.NaN;
                image.Source = string.IsNullOrEmpty(value) ? null : new BitmapImage(new Uri(value));
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
            if (string.IsNullOrEmpty(_ImgageUrl))
            {
                Data.Message.ShowMessage("链接为空");
            }
            else
            {
                if (MainPage.Global.IsAskEachTime) Data.File.SaveFile(_ImgageUrl);
                else Data.File.SaveFileWithoutDialog(_ImgageUrl);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            image.Width = double.NaN;
            image.Height = double.NaN;
            ImageUrl = _ImgageUrl;
        }

        private void Image_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Image_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void MenuFlyoutCopyImage_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(_ImgageUrl)));
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
