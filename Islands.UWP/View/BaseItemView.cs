using Islands.UWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
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
        private static string ImageViewName = "ImageView";
        private static string GifTextViewName = "GifTextView";
        public BaseItemView()
        {
            this.DefaultStyleKey = typeof(BaseItemView);
        }

        #region DependencyProperty
        public FrameworkElement TopContent
        {
            get { return (FrameworkElement)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }
        
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(FrameworkElement), typeof(BaseItemView), new PropertyMetadata(null));

        public FrameworkElement BottomContent
        {
            get { return (FrameworkElement)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }
        
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(FrameworkElement), typeof(BaseItemView), new PropertyMetadata(null));
        #endregion       
       
        public string ItemNo { get; set; }
        public string ItemThumb { get; set; }
        public string ItemImage { get; set; }    
        public string Host { get; set; }
        public string GetRefAPI { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPo { get; set; }
        public bool NoImage { get; set; }     
        public bool IsLocalImage { get; set; }
        public bool IsTextSelectionEnabled { get; set; }

        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;
        
        private Button showImageButton { get; set; }
        private ProgressRing progressRing { get; set; }
        private Grid imageView { get; set; }
        private Image image { get; set; }
        private Grid gifTextView { get; set; }
        protected IslandsCode IslandCode { get; set; }

        protected void BaseInit(ViewModel.ItemViewModel itemViewModel)
        {
            Host = itemViewModel.Host;
            GetRefAPI = itemViewModel.GetRefAPI;
            ItemNo = itemViewModel.ItemNo;
            ItemThumb = itemViewModel.ItemThumb;
            ItemImage = itemViewModel.ItemImage;
            IsAdmin = itemViewModel.IsAdmin;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            showImageButton = GetTemplateChild(ShowImageButtonName) as Button;
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            image = GetTemplateChild(ImageName) as Image;
            imageView = GetTemplateChild(ImageViewName) as Grid;
            gifTextView = GetTemplateChild(GifTextViewName) as Grid;
            image.Tag = ItemImage;
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                if (!NoImage) ShowImage();
                else showImageButton.Visibility = Visibility.Visible;
                showImageButton.Click += ShowImageButton_Click;
                image.Tapped += Image_Tapped;
                image.PointerPressed += Image_PointerPressed;
                image.ImageOpened += Image_ImageOpened;
                image.ImageFailed += Image_ImageFailed;
            }
        }        

        public void SetRefClick(RichTextBlock rtb)
        {
            if (rtb == null) return;
            foreach (var block in rtb.Blocks)
            {
                Paragraph p = block as Paragraph;
                if (p != null)
                {
                    foreach (var inline in p.Inlines)
                    {
                        Hyperlink h = inline as Hyperlink;
                        if (h != null && h.UnderlineStyle == UnderlineStyle.None)
                        {
                            h.Click += Ref_Click;
                        }
                    }
                }
            }
        }

        virtual protected void OnRefClick(string RefText)
        {
        }

        private void Ref_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            string refText = string.Empty;
            Hyperlink h = sender as Hyperlink;
            if (h != null && h.Inlines.Count > 0)
            {
                Run r = h.Inlines[0] as Run;
                if (r != null)
                {
                    refText = r.Text;
                }
            }
            OnRefClick(refText);
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImageTapped?.Invoke(sender, e);
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
                progressRing.Visibility = imageView.Visibility = Visibility.Visible;
                showImageButton.Visibility = Visibility.Collapsed;
                //if(IsLocalImage) Data.File.SetLocalImage(image, ItemImage);
                //else
                image.Source = new BitmapImage(new Uri(ItemThumb));
            }
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            var bitmap = image.Source as BitmapImage;
            if (bitmap != null && (bitmap.PixelWidth < Config.MaxImageWidth && bitmap.PixelHeight < Config.MaxImageHeight)) image.Stretch = Stretch.None;
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
            if (ItemImage.ToLower().EndsWith(".gif")) gifTextView.Visibility = Visibility.Visible;
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                image.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
            }
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
        }

        protected override void OnDisconnectVisualChildren()
        {
            base.OnDisconnectVisualChildren();
            RemoveEvent();
        }

        private void RemoveEvent()
        {
            showImageButton.Click -= ShowImageButton_Click;
            image.Source = null;
            image.Tapped -= Image_Tapped;
            image.PointerPressed -= Image_PointerPressed;
            image.ImageOpened -= Image_ImageOpened;
            image.ImageFailed -= Image_ImageFailed;
        }
    }
}
