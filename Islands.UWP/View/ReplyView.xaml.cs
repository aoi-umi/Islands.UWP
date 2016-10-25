using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplyView : BaseItemView
    {
        public ReplyView(Model.ReplyModel reply, IslandsCode islandCode)
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            this.reply = reply;
            this.islandCode = islandCode;
            ItemTitle = reply.title;
            ItemEmail = reply.email;
            ItemName = reply.name;
            ItemNo = reply.id;
            ItemContent = reply.content;
            switch (islandCode)
            {
                case IslandsCode.A:
                    if (reply.admin == "1") txtUserid.Foreground = Config.AdminColor;
                    if (!string.IsNullOrEmpty(reply.img))
                    {
                        ItemThumb = (Config.A.PictureHost + "thumb/" + reply.img + reply.ext);
                        ItemImage = (Config.A.PictureHost + "image/" + reply.img + reply.ext);
                    }
                    ItemCreateDate = reply.now;
                    ItemUid = reply.userid;
                    break;
                case IslandsCode.Beitai:
                    if (reply.admin == "1") txtUserid.Foreground = Config.AdminColor;
                    if (!string.IsNullOrEmpty(reply.img))
                    {
                        ItemThumb = (Config.B.PictureHost + "thumb/" + reply.img + reply.ext);
                        ItemImage = (Config.B.PictureHost + "image/" + reply.img + reply.ext);
                    }
                    ItemCreateDate = reply.now;
                    ItemUid = reply.userid;
                    break;
                case IslandsCode.Koukuko:
                    if (reply.uid.IndexOf("<font color=\"red\">") >= 0)
                    {
                        txtUserid.Foreground = Config.AdminColor;
                        reply.uid = Regex.Replace(reply.uid, "</?[^>]*/?>", "");
                    }
                    if (string.IsNullOrEmpty(reply.thumb)) ItemThumb = (Config.K.PictureHost + reply.thumb);
                    if (string.IsNullOrEmpty(reply.image)) ItemImage = (Config.K.PictureHost + reply.image);
                    ItemCreateDate = new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(Convert.ToDouble(reply.createdAt)).ToString("yyyy-MM-dd HH:mm:ss");
                    ItemUid = reply.uid;
                    break;
            }
            NoImage = MainPage.Global.NoImage;            
        }

        public RichTextBlock replyContent
        {
            get
            {
                if (rtb == null)
                {
                    string host = string.Empty;
                    switch (islandCode)
                    {
                        case IslandsCode.A: host = Config.A.Host; break;
                        case IslandsCode.Beitai: host = Config.B.Host; break;
                        case IslandsCode.Koukuko:host = Config.K.Host; break;
                        default: rtb = new RichTextBlock(); break;
                    }
                    if (!string.IsNullOrEmpty(host)) rtb = ContentConvert(host);
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
                
        private Model.ReplyModel reply { get; set; }
        private IslandsCode islandCode { get; set; }
        private RichTextBlock rtb;

        private RichTextBlock ContentConvert(string Host)
        {
            try
            {
                //补全host
                string s = reply.content.FixHost(Host);
                //链接处理
                s = s.FixLinkTag();
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
                p.Inlines.Add(new Run() { Text = reply.content });
                p.Inlines.Add(new Run() { Text = "\r\n*转换失败:" + ex.Message, Foreground = Config.ErrorColor });
                rtb.Blocks.Add(p);
            }
            Binding b = new Binding() { Source = this.DataContext, Path = new PropertyPath("ContentFontSize") };
            BindingOperations.SetBinding(rtb, RichTextBlock.FontSizeProperty, b);
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.IsTextSelectionEnabled = true;
            rtb.IsTapEnabled = true;
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

                    //从list寻找
                    var lv = this.Parent as ReplysView;
                    if (lv != null)
                    {
                        foreach (var lvi in lv.Items)
                        {
                            ThreadView tv = lvi as ThreadView;
                            if (tv != null && tv.thread != null && tv.thread.id == id)
                            {
                                ThreadView thread = new ThreadView(tv.thread, islandCode) { Margin = new Thickness(0, 0, 5, 0), Background = null, IsTextSelectionEnabled = true };
                                await Data.Message.ShowRef(r.Text, thread);
                                return;
                            }
                            ReplyView rv = lvi as ReplyView;
                            if (rv != null && rv.reply != null && rv.reply.id == id)
                            {
                                ReplyView reply = new ReplyView(rv.reply, islandCode) { Margin = new Thickness(0, 0, 5, 0) };
                                await Data.Message.ShowRef(r.Text, reply);
                                return;
                            }
                        }
                    }

                    //用api
                    string req = "";
                    try
                    {
                        switch (islandCode)
                        {
                            case IslandsCode.A: req = String.Format(Config.A.GetRefAPI, Config.A.Host, id); break;
                            case IslandsCode.Koukuko: req = String.Format(Config.K.GetRefAPI, Config.K.Host, id); break;
                            case IslandsCode.Beitai: req = String.Format(Config.B.GetRefAPI, Config.B.Host, id); break;
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
    }
}
