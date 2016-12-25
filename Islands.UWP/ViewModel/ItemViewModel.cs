using Islands.UWP.Model;
using System;
using Windows.UI.Xaml;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Data;

namespace Islands.UWP.ViewModel
{
    public class ItemViewModel
    {
        public MyGlobal GlobalConfig { get; set; }
        public BaseItemModel BaseItem { get; set; }
        public Visibility IsHadTitle { get; private set; }
        public Visibility IsHadEmail { get; private set; }
        public Visibility IsHadName { get; private set; }
        public RichTextBlock ItemContentView { get; private set; }
        public string Host { get; set; }
        public string GetRefAPI { get; set; }
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
        public bool IsAdmin { get; private set; }
        public bool IsTextSelectionEnabled { get; set; }
        private IslandsCode IslandCode { get; set; }
        public ItemViewModel(MyGlobal global, BaseItemModel baseItem)
        {
            GlobalConfig = global;
            Init(baseItem);
            InitVisibility();
            if (ItemContentView == null)
            {
                ItemContentView = ContentConvert(ItemContent);
            }
        }

        public ItemViewModel(SendModel myReply)
        {
            ItemTitle = string.IsNullOrEmpty(myReply.sendTitle) ? "" : "标题:" + myReply.sendTitle;
            ItemEmail = string.IsNullOrEmpty(myReply.sendEmail) ? "" : "email:" + myReply.sendEmail;
            ItemName = string.IsNullOrEmpty(myReply.sendName) ? "" : "名字:" + myReply.sendName;
            ItemNo = myReply.ThreadId;
            ItemCreateDate = myReply.sendDateTime;
            ItemMsg = (myReply.isMain ? "新串：" : "回复：") + myReply.sendId;
            ItemThumb = ItemImage = myReply.sendImage;
            ItemContent = myReply.sendContent;
            InitVisibility();
        }

        private void InitVisibility()
        {
            IsHadTitle = GetVisibility(ItemTitle, new string[] { "标题:", "无标题" });
            IsHadEmail = GetVisibility(ItemEmail, new string[] { "email:" });
            IsHadName = GetVisibility(ItemName, new string[] { "名字:", "无名氏" });
        }

        private Visibility GetVisibility(string str, string[] list)
        {
            if (string.IsNullOrWhiteSpace(str))
                return Visibility.Collapsed;
            foreach (var s in list)
            {
                if (s == str) return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
        private RichTextBlock ContentConvert(string content)
        {
            RichTextBlock rtb;
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
            UmiAoi.UWP.Helper.BindingHelper(new UmiAoi.UWP.BindingModel()
            {
                Source = GlobalConfig,
                Path = "ContentFontSize",
                BindingElement = rtb,
                Property = RichTextBlock.FontSizeProperty,
                BindingMode = BindingMode.OneWay
            });
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.IsTextSelectionEnabled = IsTextSelectionEnabled;
            return rtb;
        }
        private void Init(BaseItemModel baseItemModel)
        {
            #region Init
            IslandCode = baseItemModel.islandCode;
            ItemTitle = baseItemModel.title;
            ItemEmail = baseItemModel.email;
            ItemName = baseItemModel.name;
            ItemNo = baseItemModel.id;
            ItemContent = baseItemModel.content;
            switch (baseItemModel.islandCode)
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
                    GetRefAPI = Config.A.GetRefAPI;
                    Host = Config.A.Host;
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
                    GetRefAPI = Config.K.GetRefAPI;
                    Host = Config.K.Host;
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
                    GetRefAPI = Config.B.GetRefAPI;
                    Host = Config.B.Host;
                    break;
            }
            #endregion
        }
    }
}
