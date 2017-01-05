using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
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

        public Visibility RefBackroundVisibility
        {
            get { return (Visibility)GetValue(RefBackroundVisibilityProperty); }
            set { SetValue(RefBackroundVisibilityProperty, value); }
        }
        
        public static readonly DependencyProperty RefBackroundVisibilityProperty =
            DependencyProperty.Register(nameof(RefBackroundVisibility), typeof(Visibility), typeof(BaseItemView), new PropertyMetadata(Visibility.Collapsed));    
        #endregion

        public string ItemThumb { get { return ViewModel == null ? null : ViewModel.ItemThumb; } }
        public string ItemImage { get { return ViewModel == null ? null : ViewModel.ItemImage; } }
        public string Host { get { return ViewModel == null ? null : ViewModel.Host; } }
        public string GetRefAPI { get { return ViewModel == null ? null : ViewModel.GetRefAPI; } }
        //public bool IsTextSelectionEnabled { get { return ViewModel == null ? false : ViewModel.IsTextSelectionEnabled; } }
        public bool IsRef { get { return ViewModel == null ? false : ViewModel.IsRef; } }
        public bool NoImage { get; set; }
        public bool IsLocalImage { get; set; }
        private DataTypes dataType { get { return ViewModel == null ? DataTypes.None : ViewModel.DataType; } }

        private Button showImageButton { get; set; }
        private ProgressRing progressRing { get; set; }
        private Grid imageView { get; set; }
        private Image image { get; set; }
        private Grid gifTextView { get; set; }
        //private MenuFlyout itemMenuFlyout { get; set; }
        //protected IslandsCode IslandCode { get; set; }
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
            //RightTapped += BaseItemView_RightTapped;
            //itemMenuFlyout = new MenuFlyout();
            //itemMenuFlyout.Items.Add(new MenuFlyoutItem() { Text = "引用" });

            //foreach(var menuItem in itemMenuFlyout.Items)
            //{
            //    ((MenuFlyoutItem)menuItem).Click += (sender, e) =>
            //    {
            //    };
            //}
            hadApplyTemplate = true;
            ImageInit();
        }

        //private void BaseItemView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        //{
        //    var ele = sender as UIElement;
        //    itemMenuFlyout.ShowAt(ele, e.GetPosition(ele));
        //}

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
            if (ViewModel != null)
                SetRefClick(ViewModel.ItemContentView);
        }

        private void ImageInit()
        {
            if (hadApplyTemplate && !hadInitImage && !string.IsNullOrEmpty(ItemThumb))
            {
                if (!NoImage && !IsRef && dataType != DataTypes.Mark) ShowImage();
                else showImageButton.Visibility = Visibility.Visible;
                gifTextView.Visibility = Visibility.Collapsed;
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
                    var insertList = new List<InsertRefModel>();
                    foreach (var inline in p.Inlines)
                    {
                        Hyperlink h = inline as Hyperlink;
                        if (h != null && h.UnderlineStyle == UnderlineStyle.None)
                        {
                            var model = new InsertRefModel()
                            {
                                InsertAfterInline = h,
                                InsertInline = GetInsertInlines(h, ViewModel.ParentList)
                            };
                            if (model.InsertInline != null)
                                insertList.Add(model);
                            h.Click += Ref_Click;
                        }
                    }
                    insertList.ForEach(x =>
                    {
                        var index = p.Inlines.IndexOf(x.InsertAfterInline);
                        p.Inlines.Insert(index + 1, new LineBreak());
                        p.Inlines.Insert(index + 2, x.InsertInline);
                        p.Inlines.Insert(index + 3, new LineBreak());
                    });
                }
            }
        }

        private Inline GetInsertInlines(Hyperlink h, List<DataModel> list)
        {
            Inline inline = null;
            try
            {
                var run = h.Inlines[0] as Run;
                if (list == null || run == null) return inline;
                var id = run.Text.ToLower().Replace(">>", "").Replace("no.", "");
                var content = GetRefByList(id) as FrameworkElement;
                if(content != null)
                {
                    content.Margin = new Thickness(15, 0, 5, 0);
                    var i = new InlineUIContainer();
                    i.Child = content;
                    inline = i;
                }
            }
            catch (Exception ex)
            {
            }
            return inline;
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
            if (!string.IsNullOrEmpty(refText))
            {
                OnRefClick(refText);
                
            }
        }

        protected object GetRefByList(string RefText, bool returnOriginModel = false, bool showRefBackground = true)
        {
            var id = RefText;
            var list = ViewModel.ParentList;
            if (list == null) return null;
            var match = list.Find(x =>
            {
                var basemodel = x.Data as BaseItemModel;
                if (basemodel != null && basemodel.id == id) return true;
                return false;
            });
            if (match != null)
            {

                if (returnOriginModel) return match.Data;
                var content = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                content.DataContext = match;
                if (match.DataType == DataTypes.Thread)
                {
                    match.Parameter = new ItemParameter()
                    {
                        IsRef = showRefBackground,
                        IsTextSelectionEnabled = true,
                        IsPo = true,
                    };
                    content.ContentTemplate = ItemDataTemplateSelector.GetTemplate(DataTypes.Thread);
                }
                else if (match.DataType == DataTypes.Reply)
                {
                    match.Parameter = new ItemParameter()
                    {
                        IsRef = showRefBackground,
                        IsTextSelectionEnabled = true,
                        ParentList = list,
                    };
                    content.ContentTemplate = ItemDataTemplateSelector.GetTemplate(DataTypes.Reply);
                }
                return content;
            }
            return null;
        }

        protected async Task<object> GetRefByApi(string RefText)
        {
            if (ViewModel == null || ViewModel.BaseItem == null) throw new Exception("获取失败");
            string req = String.Format(GetRefAPI, Host, RefText);
            string res = await Data.Http.GetData(req);
            ReplyModel rm = Data.Convert.RefStringToReplyModel(res, ViewModel.BaseItem.islandCode);
            ReplyView reply = new ReplyView() { Reply = rm };
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
                LoadingToggle(true);
                imageView.Visibility = Visibility.Visible;
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
            LoadingToggle(false);            
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                image.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
            }
            LoadingToggle(false);
        }

        private void LoadingToggle(bool loading)
        {
            progressRing.IsActive = loading;
            if (loading)
            {
                progressRing.Visibility = Visibility.Visible;
                gifTextView.Visibility = Visibility.Collapsed;
            }
            else
            {
                progressRing.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(ItemImage) && 
                    ItemImage.ToLower().EndsWith(".gif")) gifTextView.Visibility = Visibility.Visible;
                else gifTextView.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnDisconnectVisualChildren()
        {
            base.OnDisconnectVisualChildren();
            RemoveEvent();
        }
    }

    public class InsertRefModel
    {
        public Inline InsertAfterInline { get; set; }
        public Inline InsertInline { get; set; }
    }
}
