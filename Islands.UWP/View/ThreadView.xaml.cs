using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadView : BaseItemView
    {
        public ThreadView(Model.ThreadModel thread, IslandsCode islandCode)
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            this.thread = thread;
            this.islandCode = islandCode;
            #region Init
            ItemTitle = thread.title;
            ItemEmail = thread.email;
            ItemName = thread.name;
            ItemNo = thread.id;
            ItemReplyCount = thread.replyCount;
            ItemContent = thread.content;
            switch (islandCode)
            {
                case IslandsCode.A:
                    if (thread.admin == "1") txtUserid.Foreground = Config.AdminColor;
                    if (!string.IsNullOrEmpty(thread.img))
                    {
                        ItemThumb = (Config.A.PictureHost + "thumb/" + thread.img + thread.ext);
                        ItemImage = (Config.A.PictureHost + "image/" + thread.img + thread.ext);
                    }
                    ItemCreateDate = thread.now;
                    ItemUid = thread.userid;
                    break;
                case IslandsCode.Beitai:
                    if (thread.admin == "1") txtUserid.Foreground = Config.AdminColor;
                    if (!string.IsNullOrEmpty(thread.img))
                    {
                        ItemThumb = (Config.B.PictureHost + "thumb/" + thread.img + thread.ext);
                        ItemImage = (Config.B.PictureHost + "image/" + thread.img + thread.ext);
                    }
                    ItemCreateDate = thread.now;
                    ItemUid = thread.userid;
                    break;
                case IslandsCode.Koukuko:
                    if (thread.uid.IndexOf("<font color=\"red\">") >= 0)
                    {
                        txtUserid.Foreground = Config.AdminColor;
                        thread.uid = Regex.Replace(thread.uid, "</?[^>]*/?>", "");
                    }
                    if (string.IsNullOrEmpty(thread.thumb)) ItemThumb = (Config.K.PictureHost + thread.thumb);
                    if (string.IsNullOrEmpty(thread.image)) ItemImage = (Config.K.PictureHost + thread.image);
                    ItemCreateDate = new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(Convert.ToDouble(thread.createdAt)).ToString("yyyy-MM-dd HH:mm:ss");
                    ItemUid = thread.uid;
                    break;
            }
            #endregion
            NoImage = MainPage.Global.NoImage;
            if (!string.IsNullOrEmpty(ItemImage))
            {
                imageBox.Tag = ItemImage;
                imageBox.Tapped += ImageBox_Tapped;
                imageBox.PointerPressed += ImageBox_PointerPressed;
            }
        }

        public event ImageTappedEventHandler ImageTapped;
        public Model.ThreadModel thread { get; set; }                                       
        public RichTextBlock threadContent
        {
            get
            {
                string host = string.Empty;
                if (rtb == null)
                {
                    switch (islandCode)
                    {
                        case IslandsCode.A:host = Config.A.Host; break;
                        case IslandsCode.Beitai: host = Config.B.Host; break;
                        case IslandsCode.Koukuko: host = Config.K.Host; break;
                        default: rtb = new RichTextBlock(); break;
                    }
                    if(!string.IsNullOrEmpty(host)) rtb = ContentConvert(host);
                }
                return rtb;
            }
        }

        public bool IsPo
        {
            set
            {
                if (value) txtUserid.Foreground = Config.PoColor;
            }
        }
        public bool IsTextSelectionEnabled { get; set; }
        public bool NoImage
        {
            get
            {
                return ShowImageButton.Visibility == Visibility.Visible ? true : false;
            }
            set
            {
                if (!string.IsNullOrEmpty(ItemThumb))
                {
                    if (value)
                    {
                        ShowImageButton.Visibility = Visibility.Visible;
                        LoadingView.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ShowImageButton.Visibility = Visibility.Collapsed;
                        LoadingView.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        
        private RichTextBlock rtb;
        private IslandsCode islandCode { get; set; }
        private RichTextBlock ContentConvert(string Host)
        {
            try
            {
                //补全host
                string s = thread.content.FixHost(Host);
                //链接处理
                s = s.FixLinkTag();
                //if (islandCode == IslandsCode.Koukuko) s = Regex.Replace(s, "\\[ref tid=\"(\\d+)\"/\\]", "&gt;&gt;No.$1");
                s = HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(s, true);
                //引用处理
                s = s.FixRef();
                s = s.Replace("&#xFFFF;", "");
                if (islandCode == IslandsCode.Koukuko) s = s.FixEntity();
                rtb = (RichTextBlock)XamlReader.Load(s);
            }
            catch (Exception ex)
            {
                rtb = new RichTextBlock();
                Paragraph p = new Paragraph();
                p.Inlines.Add(new Run() { Text = thread.content });
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

        private async void Ref_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Hyperlink h = sender as Hyperlink;
            if (h != null && h.Inlines.Count > 0)
            {
                Run r = h.Inlines[0] as Run;
                if (r != null)
                {
                    string id = r.Text.ToLower().Replace(">>", "").Replace("no.", "");
                    
                    //用api
                    string req = "";
                    try
                    {
                        switch (islandCode)
                        {
                            case IslandsCode.A: req = String.Format(Config.A.GetRefAPI, Config.A.Host, id); break;
                            case IslandsCode.Koukuko: req = String.Format(Config.K.GetRefAPI, Config.K.Host, id); break;
                            case IslandsCode.Beitai: req  = String.Format(Config.B.GetRefAPI, Config.B.Host, id); break;
                        }
                        string res = await Data.Http.GetData(req);
                        Model.ReplyModel rm = Data.Convert.RefStringToReplyModel(res, islandCode);
                        ReplyView reply = new ReplyView(rm, islandCode) { Margin = new Thickness(0, 0, 5, 0) };
                        await Data.Message.ShowRef(r.Text, reply);
                    }
                    catch (Exception ex)
                    {
                        Data.Message.ShowMessage(ex.Message);
                        return;
                    }
                }
            }
        }

        private void ImageBox_Opened(object sender, RoutedEventArgs e)
        {
            var bitmap = imageBox.Source as BitmapImage;
            if (bitmap != null && bitmap.PixelWidth < Config.MaxImageWidth && bitmap.PixelHeight < Config.MaxImageHeight) imageBox.Stretch = Stretch.None;
            LoadingView.IsActive = false;
        }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                imageBox.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
                LoadingView.IsActive = false;
            }
        }

        private void ImageBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImageTapped?.Invoke(sender, e);
        }

        private void ImageBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void ShowImage_Click(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        public void ShowImage()
        {
            if (!string.IsNullOrEmpty(ItemThumb))
            {
                LoadingView.Visibility = Visibility.Visible;
                LoadingView.IsActive = true;
                NoImage = false;
                imageBox.Source = new BitmapImage(new Uri(ItemThumb));
            }
        }
    }
}
