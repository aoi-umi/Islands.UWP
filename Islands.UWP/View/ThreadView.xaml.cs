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
    public sealed partial class ThreadView : UserControl
    {
        public ThreadView(Model.ThreadModel thread, IslandsCode islandCode)
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            this.thread = thread;
            this.islandCode = islandCode;
            NoImage = MainPage.Global.NoImage;
            if (!string.IsNullOrEmpty(this.threadThumb))
            {
                imageBox.Tag = this.threadImage;
                imageBox.Tapped += ImageBox_Tapped;
                imageBox.PointerPressed += ImageBox_PointerPressed;
            }
        }

        public Model.ThreadModel thread { get; set; }
        public Visibility threadIsHadTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(threadTitle) && threadTitle != "标题:" && threadTitle != "无标题")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility threadIsHadEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(threadEmail) && threadEmail != "email:")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
        public Visibility threadIsHadName
        {
            get
            {
                if (!string.IsNullOrEmpty(threadName) && threadName != "名字:" && threadName != "无名氏")
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public string threadTitle
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return /*"标题:" +*/ thread.title;
                    default: return "";
                }
            }
        }
        public string threadEmail
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return /*"email:" +*/ thread.email;
                    default: return "";
                }
            }
        }
        public string threadName
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return /*"名字:" +*/ thread.name;
                    default: return "";
                }
            }
        }
        public string threadNo
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return thread.id;
                    default: return "";
                }
            }
        }
        public string threadCreateDate
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai: return thread.now;
                    case IslandsCode.Koukuko:
                        DateTime dt = new DateTime(1970, 1, 1).ToLocalTime();
                        return dt.AddMilliseconds(Convert.ToDouble(thread.createdAt)).ToString("yyyy-MM-dd HH:mm:ss");
                    default: return "";
                }
            }
        }
        public string threadUid
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        if (thread.admin == "1")
                        {
                            txtUserid.Foreground = Config.AdminColor;
                        }
                        return thread.userid;
                    case IslandsCode.Koukuko:
                        if (thread.uid.IndexOf("<font color=\"red\">") >= 0)
                        {
                            txtUserid.Foreground = Config.AdminColor;
                            thread.uid = Regex.Replace(thread.uid, "</?[^>]*/?>", "");
                        }
                        return thread.uid;
                    default: return "";
                }
            }
        }
        public string threadReplyCount
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                    case IslandsCode.Koukuko: return thread.replyCount;
                    default: return "";
                }
            }
        }
        public string threadThumb
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.A.PictureHost + "thumb/" + thread.img + thread.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.B.PictureHost + "thumb/" + thread.img + thread.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(thread.thumb))
                            return "";
                        return (Config.K.PictureHost + thread.thumb);
                    default: return "";
                }
            }
        }
        public string threadImage
        {
            get
            {
                switch (islandCode)
                {
                    case IslandsCode.A:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.A.PictureHost + "image/" + thread.img + thread.ext);
                    case IslandsCode.Beitai:
                        if (string.IsNullOrEmpty(thread.img))
                            return "";
                        return (Config.B.PictureHost + "image/" + thread.img + thread.ext);
                    case IslandsCode.Koukuko:
                        if (string.IsNullOrEmpty(thread.image))
                            return "";
                        return (Config.K.PictureHost + thread.image);
                    default: return "";
                }
            }
        }
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
                if (!string.IsNullOrEmpty(this.threadThumb))
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

        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;

        public void ShowImage()
        {
            if (!string.IsNullOrEmpty(this.threadThumb))
            {
                LoadingView.Visibility = Visibility.Visible;
                LoadingView.IsActive = true;
                NoImage = false;
                imageBox.Source = new BitmapImage(new Uri(this.threadThumb));
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
                            if (h != null)
                            {
                                if (h.UnderlineStyle == UnderlineStyle.None) h.Click += Ref_Click;
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

                        JObject jObj;
                        if (!Data.Json.TryDeserializeObject(res, out jObj)) throw new Exception(res.UnicodeDencode());

                        Model.ReplyModel rm = null;
                        switch (islandCode)
                        {
                            case IslandsCode.A:
                            case IslandsCode.Beitai:
                                rm = Data.Json.Deserialize<Model.ReplyModel>(res); break;
                            case IslandsCode.Koukuko:
                                Model.KReplyQueryResponse kResModel = Data.Json.Deserialize<Model.KReplyQueryResponse>(res);
                                if (kResModel != null)
                                {
                                    if (!kResModel.success) throw new Exception(kResModel.message);
                                    rm = kResModel.data;
                                }
                                break;
                        }
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

        private void ImageBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ImageTapped != null)
                ImageTapped(sender, e);
        }

        private void ImageBox_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(threadThumb))
            {
                imageBox.Source = new BitmapImage(new Uri(Config.FailedImageUri, UriKind.RelativeOrAbsolute));
                LoadingView.IsActive = false;
            }
        }

        private void ImageBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void ShowImage_Click(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        private void ImageBox_Opened(object sender, RoutedEventArgs e)
        {
            var bitmap = imageBox.Source as BitmapImage;
            if (bitmap != null && bitmap.PixelWidth < Config.MaxImageWidth && bitmap.PixelHeight < Config.MaxImageHeight) imageBox.Stretch = Stretch.None;
            LoadingView.IsActive = false;
        }
    }
}
