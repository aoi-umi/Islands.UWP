using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private static string ImageViewName = "ImageView";
        private static string GifTextViewName = "GifTextView";
        public BaseItemView() : base()
        {
            this.DefaultStyleKey = typeof(BaseItemView);
            //Loaded += BaseItemView_Loaded;
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

        public ItemViewModel ViewModel
        {
            get { return (ItemViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ItemViewModel), typeof(BaseItemView), 
                new PropertyMetadata(null, new PropertyChangedCallback(OnViewModelChanged)));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ele = d as BaseItemView;
            if (e.NewValue != e.OldValue)
            {
                ele.hadInitImage = false;
                ele.OnViewModelChanged();
            }
        }
        #endregion
        
        public string ItemThumb { get { return ViewModel == null ? null : ViewModel.ItemThumb; } }
        public string ItemImage { get { return ViewModel == null ? null : ViewModel.ItemImage; } }
        public string Host { get { return ViewModel == null ? null : ViewModel.Host; } }
        public string GetRefAPI { get { return ViewModel == null ? null : ViewModel.GetRefAPI; } }
        public bool IsTextSelectionEnabled { get { return ViewModel == null ? false : ViewModel.IsTextSelectionEnabled; } }
        public bool NoImage { get; set; }
        public bool IsLocalImage { get; set; }
        private DataTypes dataType { get { return ViewModel == null ? DataTypes.None : ViewModel.DataType; } }

        private Button showImageButton { get; set; }
        private ProgressRing progressRing { get; set; }
        private Grid imageView { get; set; }
        private Image image { get; set; }
        private Grid gifTextView { get; set; }
        protected IslandsCode IslandCode { get; set; }
        private bool hadApplyTemplate { get; set; }
        private bool hadInitImage { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            showImageButton = GetTemplateChild(ShowImageButtonName) as Button;
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            image = GetTemplateChild(ImageName) as Image;
            imageView = GetTemplateChild(ImageViewName) as Grid;
            gifTextView = GetTemplateChild(GifTextViewName) as Grid;
            hadApplyTemplate = true;
            ImageInit();
        }

        private void RemoveEvent()
        {
            showImageButton.Click -= ShowImageButton_Click;
            image.Source = null;
            image.PointerPressed -= Image_PointerPressed;
            image.ImageOpened -= Image_ImageOpened;
            image.ImageFailed -= Image_ImageFailed;
        }

        virtual protected void OnViewModelChanged()
        {
            ImageInit();
            if (IsTextSelectionEnabled && ViewModel.ItemContentView != null)
                SetRefClick(ViewModel.ItemContentView);
        }

        private void ImageInit()
        {
            if (hadApplyTemplate && !hadInitImage && !string.IsNullOrEmpty(ItemThumb))
            {
                if (!NoImage && dataType != DataTypes.Mark) ShowImage();
                else showImageButton.Visibility = Visibility.Visible;
                showImageButton.Click += ShowImageButton_Click;
                image.PointerPressed += Image_PointerPressed;
                image.ImageOpened += Image_ImageOpened;
                image.ImageFailed += Image_ImageFailed;
                hadInitImage = true;
            }
        }

        private void SetRefClick(RichTextBlock rtb)
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
            refText = refText.ToLower().Replace(">>", "").Replace("no.", "");
            if(!string.IsNullOrEmpty(refText))
                OnRefClick(refText);
        }

        protected object GetRefByList(string RefText)
        {
            var list = Helper.GetParent(this, typeof(ReplysView)) as ReplysView;
            if (list != null)
            {
                foreach (var lvi in list.Items)
                {
                    var model = lvi as DataModel;
                    if (model != null)
                    {
                        var item = model.Data as BaseItemModel;
                        if (item != null && item.id == RefText)
                        {
                            if (item is ThreadModel)
                            {
                                ThreadView thread = new ThreadView() { Thread = item as ThreadModel };
                                return thread;
                            }
                            else if (item is ReplyModel)
                            {
                                ReplyView reply = new ReplyView() { Reply = item as ReplyModel };                                
                                return reply;
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected async Task<object> GetRefByApi(string RefText)
        {
            string req = String.Format(GetRefAPI, Host, RefText);
            string res = await Data.Http.GetData(req);
            ReplyModel rm = Data.Convert.RefStringToReplyModel(res, IslandCode);
            rm.islandCode = IslandCode;
            ReplyView reply = new ReplyView() { Reply = rm, Margin = new Thickness(0, 0, 5, 0) };
            return reply;
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
            else gifTextView.Visibility = Visibility.Collapsed;
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
    }
}
