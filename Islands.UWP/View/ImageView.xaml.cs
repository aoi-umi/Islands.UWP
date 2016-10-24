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

        private string _imgageUrl { get; set; }
        public string imageUrl
        {
            set
            {
                IsLoading = true;
                _imgageUrl = value;
                image.Width = 200;
                image.Height = double.NaN;
                image.Source = string.IsNullOrEmpty(_imgageUrl) ? null : new BitmapImage(new Uri(value));
            }
        }

        private void ImageView_ImageOpened(object sender, RoutedEventArgs e)
        {
            IsLoading = false;
        }

        private void ImageView_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            imageUrl = Config.FailedImageUri;
            IsLoading = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_imgageUrl))
            {
                Data.Message.ShowMessage("链接为空");
            }
            else
            {
                if (MainPage.Global.IsAskEachTime) Data.File.SaveFile(_imgageUrl);
                else Data.File.SaveFileWithoutDialog(_imgageUrl);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            image.Width = double.NaN;
            image.Height = double.NaN;
            imageUrl = _imgageUrl;
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
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(_imgageUrl)));
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
