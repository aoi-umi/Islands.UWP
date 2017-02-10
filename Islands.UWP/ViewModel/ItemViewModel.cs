using Islands.UWP.Model;
using System;
using Windows.UI.Xaml;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;

namespace Islands.UWP.ViewModel
{
    public class ItemViewModel
    {
        public MyGlobal GlobalConfig { get; set; }
        public DataTypes DataType { get; set; }
        public List<DataModel> ParentList { get; set; }
        private BaseItemModel _BaseItem { get; set; }
        public BaseItemModel BaseItem
        {
            get
            { return _BaseItem; }
            set
            {
                if (_BaseItem != value)
                {
                    _BaseItem = value;
                    InitByBaseItem();
                }
            }
        }
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
        public bool IsPo { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRef { get; set; }
        private bool _IsTextSelectionEnabled { get; set; }
        public bool IsTextSelectionEnabled
        {
            get { return _IsTextSelectionEnabled; }
            set {
                _IsTextSelectionEnabled = value;
                if (ItemContentView != null)
                    ItemContentView.IsTextSelectionEnabled = value;
            }
        }
        public Brush UserColor
        {
            get
            {
                if (IsAdmin) return Config.AdminColor;
                else if (IsPo) return Config.PoColor;
                else return null;
            }
        }
        private IslandsCode IslandCode { get; set; }
         
        public ItemViewModel()
        {      
        }

        public ItemViewModel(BaseItemModel baseItem)
        {
            BaseItem = baseItem;
        }

        private void InitByBaseItem()
        {
            if (BaseItem == null) BaseItem = new BaseItemModel();
            Init(BaseItem);
            ResetString();
            if (ItemContentView == null)
            {
                ItemContentView = ContentConvert(ItemContent);
            }
        }

        public ItemViewModel(SendModel myReply)
        {
            if (myReply == null) return;
            ItemTitle = string.IsNullOrEmpty(myReply.sendTitle) ? "" : "标题:" + myReply.sendTitle;
            ItemEmail = string.IsNullOrEmpty(myReply.sendEmail) ? "" : "email:" + myReply.sendEmail;
            ItemName = string.IsNullOrEmpty(myReply.sendName) ? "" : "名字:" + myReply.sendName;
            ItemNo = myReply.ThreadId;
            ItemCreateDate = myReply.sendDateTime;
            ItemMsg = (myReply.isMain ? "新串：" : "回复：") + myReply.sendId;
            ItemThumb = ItemImage = myReply.sendImage;
            ItemContent = myReply.sendContent;
            ResetString();
        }

        private void ResetString()
        {
            if (CheckString(ItemTitle, new string[] { "标题:", "无标题" })) ItemTitle = null;
            if (CheckString(ItemEmail, new string[] { "email:" })) ItemEmail = null;
            if (CheckString(ItemName, new string[] { "名字:", "无名氏" })) ItemName = null;
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
            RichTextBlock rtb = null;
            if (string.IsNullOrEmpty(content)) return rtb;
            try
            {
                string Host = Config.Island[IslandCode.ToString()].Host;
                //switch (IslandCode)
                //{
                //    case IslandsCode.A: Host = Config.A.Host; break;
                //    case IslandsCode.Beitai: Host = Config.B.Host; break;
                //    case IslandsCode.Koukuko: Host = Config.K.Host; break;
                //    default: rtb = new RichTextBlock(); break;
                //}
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
            rtb.IsTextSelectionEnabled = false;
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
            
            var t = baseItemModel as ThreadModel;
            if (t != null) ItemReplyCount = t.replyCount;
            var islandConfig = Config.Island[baseItemModel.islandCode.ToString()];
            GetRefAPI = islandConfig.GetRefAPI;
            Host = islandConfig.Host;
            switch (baseItemModel.islandCode)
            {
                case IslandsCode.A:
                    if (baseItemModel.admin == "1") IsAdmin = true;
                    if (!string.IsNullOrEmpty(baseItemModel.img))
                    {
                        ItemThumb = (islandConfig.PictureHost + "thumb/" + baseItemModel.img + baseItemModel.ext);
                        ItemImage = (islandConfig.PictureHost + "image/" + baseItemModel.img + baseItemModel.ext);
                    }
                    ItemCreateDate = baseItemModel.now;
                    ItemUid = baseItemModel.userid;
                    //GetRefAPI = Config.A.GetRefAPI;
                    //Host = Config.A.Host;
                    break;
                case IslandsCode.Beitai:
                    if (baseItemModel.admin == "1") IsAdmin = true;
                    if (!string.IsNullOrEmpty(baseItemModel.img))
                    {
                        ItemThumb = (islandConfig.PictureHost + "thumb/" + baseItemModel.img + baseItemModel.ext);
                        ItemImage = (islandConfig.PictureHost + "image/" + baseItemModel.img + baseItemModel.ext);
                    }
                    ItemCreateDate = baseItemModel.now;
                    ItemUid = baseItemModel.userid;
                    //GetRefAPI = Config.B.GetRefAPI;
                    //Host = Config.B.Host;
                    break;
                case IslandsCode.Koukuko:
                    if (baseItemModel.uid.IndexOf("<font color=\"red\">") >= 0)
                    {
                        IsAdmin = true;
                        baseItemModel.uid = Regex.Replace(baseItemModel.uid, "</?[^>]*/?>", "");
                    }
                    if (!string.IsNullOrEmpty(baseItemModel.thumb)) ItemThumb = (islandConfig.PictureHost + baseItemModel.thumb);
                    if (!string.IsNullOrEmpty(baseItemModel.image)) ItemImage = (islandConfig.PictureHost + baseItemModel.image);
                    ItemCreateDate = new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(Convert.ToDouble(baseItemModel.createdAt)).ToString("yyyy-MM-dd HH:mm:ss");
                    ItemUid = baseItemModel.uid;
                    //GetRefAPI = Config.K.GetRefAPI;
                    //Host = Config.K.Host;
                    break;
            }
            #endregion
        }
        private bool CheckString(string str, string[] list)
        {
            if (string.IsNullOrWhiteSpace(str))
                return true;
            foreach (var s in list)
            {
                if (s == str) return true;
            }
            return false;
        }
    }
}
