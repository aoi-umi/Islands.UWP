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
        public BaseItemView()
        {
            this.DefaultStyleKey = typeof(BaseItemView);
        }

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
        public RichTextBlock ItemContentView
        {
            get
            {
                if (rtb == null)
                {
                    rtb = ContentConvert(ItemContent);
                }
                return rtb;
            }
        }

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
        private Image image { get; set; }
        protected RichTextBlock rtb { get; set; }
        protected IslandsCode IslandCode { get; set; }

        protected void BaseInit(BaseItemModel baseItemModel)
        {
            #region Init
            ItemTitle = baseItemModel.title;
            ItemEmail = baseItemModel.email;
            ItemName = baseItemModel.name;
            ItemNo = baseItemModel.id;
            ItemContent = baseItemModel.content;
            switch (IslandCode)
            {
                case IslandsCode.A:
                    if (baseItemModel.admin == "1") IsAdmin = true;
                    if (!string.IsNullOrEmpty(baseItemModel.img))
                    {
                        ItemThumb = (Config.A.PictureHost + "thumb/" + baseItemModel.img + baseItemModel.ext);
                        ItemImage = (Config.A.PictureHost + "image/" + baseItemModel.img + baseItemModel.ext);
                    }
                    ItemCreateDate = baseItemModel.now;
                    ItemUid = baseItemModel.userid;
                    break;
                case IslandsCode.Beitai:
                    if (baseItemModel.admin == "1") IsAdmin = true;
                    if (!string.IsNullOrEmpty(baseItemModel.img))
                    {
                        ItemThumb = (Config.B.PictureHost + "thumb/" + baseItemModel.img + baseItemModel.ext);
                        ItemImage = (Config.B.PictureHost + "image/" + baseItemModel.img + baseItemModel.ext);
                    }
                    ItemCreateDate = baseItemModel.now;
                    ItemUid = baseItemModel.userid;
                    break;
                case IslandsCode.Koukuko:
                    if (baseItemModel.uid.IndexOf("<font color=\"red\">") >= 0)
                    {
                        IsAdmin = true;
                        baseItemModel.uid = Regex.Replace(baseItemModel.uid, "</?[^>]*/?>", "");
                    }
                    if (!string.IsNullOrEmpty(baseItemModel.thumb)) ItemThumb = (Config.K.PictureHost + baseItemModel.thumb);
                    if (!string.IsNullOrEmpty(baseItemModel.image)) ItemImage = (Config.K.PictureHost + baseItemModel.image);
                    ItemCreateDate = new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(Convert.ToDouble(baseItemModel.createdAt)).ToString("yyyy-MM-dd HH:mm:ss");
                    ItemUid = baseItemModel.uid;
                    break;
            }
            #endregion
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            showImageButton = GetTemplateChild(ShowImageButtonName) as Button;
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            image = GetTemplateChild(ImageName) as Image;
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
            switch (IslandCode)
            {
                case IslandsCode.A: GetRefAPI = Config.A.GetRefAPI; Host = Config.A.Host; break;
                case IslandsCode.Koukuko: GetRefAPI = Config.K.GetRefAPI; Host = Config.K.Host; break;
                case IslandsCode.Beitai: GetRefAPI = Config.B.GetRefAPI; Host = Config.B.Host; break;
            }
        }

        protected RichTextBlock ContentConvert(string content)
        {
            try
            {
                string Host = string.Empty;
                switch (IslandCode)
                {
                    case IslandsCode.A: Host = Config.A.Host; break;
                    case IslandsCode.Beitai: Host = Config.B.Host; break;
                    case IslandsCode.Koukuko: Host = Config.K.Host; break;
                    default: rtb = new RichTextBlock(); break;
                }
                //补全host
                string s = content.FixHost(Host);
                //链接处理
                s = s.FixLinkTag();
                //if (islandCode == IslandsCode.Koukuko) s = Regex.Replace(s, "\\[ref tid=\"(\\d+)\"/\\]", "&gt;&gt;No.$1");
                s = HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(s, true);
                //引用处理
                s = s.FixRef();
                s = s.Replace("&#xFFFF;", "");
                if (IslandCode == IslandsCode.Koukuko) s = s.FixEntity();
                rtb = (RichTextBlock)XamlReader.Load(s);
            }
            catch (Exception ex)
            {
                rtb = new RichTextBlock();
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run() { Text = content });
                p.Inlines.Add(new Run() { Text = "\r\n*转换失败:" + ex.Message, Foreground = Config.ErrorColor });
                rtb.Blocks.Add(p);
            }
            Binding b = new Binding() { Source = this.DataContext, Path = new PropertyPath("ContentFontSize") };
            BindingOperations.SetBinding(rtb, RichTextBlock.FontSizeProperty, b);
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.IsTextSelectionEnabled = IsTextSelectionEnabled;
            if (IsTextSelectionEnabled)
            {
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
            return rtb;
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
                progressRing.Visibility = image.Visibility = Visibility.Visible;
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
