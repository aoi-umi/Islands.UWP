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
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Islands.UWP
{
    public class BaseItemView : ContentControl
    {
        private static string ShowImageButtonName = "ShowImageButton";
        private static string ProgressRingName = "ProgressRing";
        private static string ImageName = "Image";
        public BaseItemView()
        {
            this.DefaultStyleKey = typeof(BaseItemView);
        }

        public FrameworkElement TopContent
        {
            get { return (FrameworkElement)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(FrameworkElement), typeof(BaseItemView), new PropertyMetadata(null));

        public FrameworkElement BottomContent
        {
            get { return (FrameworkElement)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(FrameworkElement), typeof(BaseItemView), new PropertyMetadata(null));

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

        public bool NoImage { get; set; }        

        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;

        private Button showImageButton { get; set; }
        private ProgressRing progressRing { get; set; }
        private Image image { get; set; }        
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            showImageButton = GetTemplateChild(ShowImageButtonName) as Button;
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            image = GetTemplateChild(ImageName) as Image;
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                if (!NoImage) ShowImage();
                else showImageButton.Visibility = Visibility.Visible;

                showImageButton.Click += ShowImageButton_Click;
                image.PointerPressed += Image_PointerPressed;
                image.ImageOpened += Image_ImageOpened;
                image.ImageFailed += Image_ImageFailed;
            }

        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void ShowImageButton_Click(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        public void ShowImage()
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                NoImage = false;
                progressRing.IsActive = true;
                progressRing.Visibility = image.Visibility = Visibility.Visible;
                showImageButton.Visibility = Visibility.Collapsed;
                image.Source = new BitmapImage(new Uri(ItemThumb));
            }
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            var bitmap = image.Source as BitmapImage;
            if (bitmap != null && !(bitmap.PixelWidth < Config.MaxImageWidth && bitmap.PixelHeight < Config.MaxImageHeight)) image.Stretch = Stretch.Uniform;
            progressRing.IsActive = false;
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                image.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
                progressRing.IsActive = false;
            }
        }

        protected override void OnDisconnectVisualChildren()
        {
            base.OnDisconnectVisualChildren();
            showImageButton.Click -= ShowImageButton_Click;
            image.ImageOpened -= Image_ImageOpened;
            image.ImageFailed -= Image_ImageFailed;
        }
    }
}
