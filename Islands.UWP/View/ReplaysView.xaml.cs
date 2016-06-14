using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplysView : UserControl
    {
        public ReplysView()
        {
            InitializeComponent();
            this.DataContext = MainPage.Global;
            replyListView.Items.Add(ReplyStatusBox);
            replyListScrollViewer.ViewChanged += ReplyListScrollViewer_ViewChanged;
            ReplyStatusBox.Tapped += ReplyStatusBox_Tapped;
        }
        
        public Model.PostRequest postReq;
        public IslandsCode islandCode;
        public int pageSize { get; set; }
        public string currThread { get; set; }      

        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;
        public delegate void MarkSuccessEventHandler(Object sender, Model.ThreadModel t);
        public event MarkSuccessEventHandler MarkSuccess;
        public delegate void MenuClickEventHandler(Object sender, RoutedEventArgs e);
        public event MenuClickEventHandler MenuClick;

        public void GetReplyListByID(string threadId, int markId)
        {
            currThread = threadId;
            try
            {
                this.markId = markId;
                replyId = threadId;
                postReq.ID = threadId;
                _Refresh(1);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        private string message
        {
            set
            {
                ReplyStatusBox.Text = value;
            }
        }
        private TextBlock ReplyStatusBox = new TextBlock()
        {
            Text = "还未看过任何串(つд⊂)",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        private int markId { get; set; }
        private int currPage { get; set; }
        private int allPage
        {
            get
            {
                if (replyCount == 0) return 0;
                return replyCount / pageSize + (replyCount % pageSize > 0 ? 1 : 0);
            }
        }
        private int replyCount { get; set; }
        private bool IsGetAllReply = false;
        private string replyId {set { Title.Text= value; } }
        private string txtReplyCount { set { ListCount.Text = "(" +value + "," + allPage + "P)"; } }
        private Model.ThreadModel top = null;
        private Model.ReplyModel lastReply = null;

        private void ReplyStatusBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            postReq.Page = currPage;
            GetReplyList(postReq, islandCode);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _Refresh(1);
        }

        private void DataLoading()
        {
            ReplyLoading.IsActive = true;
            IsHitTestVisible = false;
            replyListView.Items.Remove(ReplyStatusBox);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            replyListView.Items.Add(ReplyStatusBox);
            ReplyLoading.IsActive = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            lastReply = null;
            IsGetAllReply = false;
            replyCount = 0;
            currPage = page;
            for (var i = replyListView.Items.Count - 1; i >= 0; i--)
            {
                replyListView.Items.RemoveAt(i);
            }
            try
            {
                postReq.Page = currPage;
                GetReplyList(postReq, islandCode);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        private async void GetReplyList(Model.PostRequest req, IslandsCode code)
        {
            if (ReplyLoading.IsActive) return;
            txtReplyCount = "0";
            DataLoading();
            string res = "";
            currPage = req.Page;
            try
            {
                if (string.IsNullOrEmpty(req.ID)) throw new Exception("串号为空");
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                JObject jObj;
                if (!Data.Json.TryDeserializeObject(res, out jObj)) throw new Exception(res.UnicodeDencode());                

                top = null;
                JArray Replys = null;
                switch (code)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        top = Data.Json.Deserialize<Model.ThreadModel>(res);
                        if (jObj["replys"].HasValues)
                            Replys = Data.Json.Deserialize<JArray>(jObj["replys"].ToString());
                        break;
                    case IslandsCode.Koukuko:
                        if (jObj["success"].ToString().ToLower() != "true") throw new Exception(jObj["message"].ToString());
                        top = Data.Json.Deserialize<Model.ThreadModel>(jObj["threads"].ToString());
                        if (jObj["replys"].HasValues)
                            Replys = Data.Json.Deserialize<JArray>(jObj["replys"].ToString());
                        break;
                }
                top.islandCode = code;
                top._id = markId;                

                int _replyCount;
                int.TryParse(top.replyCount, out _replyCount);
                if (replyListView.Items.Count == 0 && top != null)
                {
                    var tv = new ThreadView(top, code) { Tag = top, IsTextSelectionEnabled = true,Background = null };
                    tv.ImageTapped += Image_ImageTapped;
                    tv.IsPo = true;
                    if(!MainPage.Global.NoImage) tv.ShowImage();
                    replyListView.Items.Add(tv);
                }
                replyCount = _replyCount;
                txtReplyCount = _replyCount.ToString();
                if (Replys == null || Replys.Count == 0)
                {
                    IsGetAllReply = true;
                    throw new Exception("已经没有了");
                }

                if((replyListView.Items.Count - 1) % (pageSize + 1) == 0)
                    replyListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var reply in Replys)
                {
                    Model.ReplyModel rm = Data.Json.Deserialize<Model.ReplyModel>(reply.ToString());
                    int index = Replys.IndexOf(reply);
                    if (lastReply == null && Replys.IndexOf(reply) == Replys.Count - 1)
                        lastReply = rm;
                    else if (lastReply != null)
                    {
                        switch (code)
                        {
                            case IslandsCode.A:
                            case IslandsCode.Beitai:
                                if (String.Compare(rm.id, lastReply.id) <= 0) continue; break;
                            case IslandsCode.Koukuko:
                                if (String.Compare(rm.id, lastReply.id) <= 0) continue; break;
                        }
                        lastReply = rm;
                    }
                    var rv = new ReplyView(rm, code);
                    if ((code == IslandsCode.Koukuko && rm.uid == top.uid) || (code != IslandsCode.Koukuko && rm.userid == top.userid))
                        rv.IsPo = true;
                    rv.ImageTapped += Image_ImageTapped;
                    replyListView.Items.Add(rv);

                }
                if (Replys.Count < pageSize || (currPage - 1) * pageSize + Replys.Count == replyCount)
                {
                    IsGetAllReply = true;
                    throw new Exception("已经没有了");
                }
                ++currPage;

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                DataLoaded();
            }

        }

        //滑动刷新
        private void ReplyListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            if (!IsGetAllReply && currPage <= allPage && sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                try
                {
                    sv.ChangeView(null, sv.VerticalOffset - 1, null);
                    postReq.Page = currPage;
                    GetReplyList(postReq, islandCode);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
        }

        private async void GotoPageButton_Click(object sender, RoutedEventArgs e)
        {
            var page = await Data.Message.GotoPageYesOrNo();
            if (page > 0)
                _Refresh(page);
        }

        //点击图片
        private void Image_ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ImageTapped != null)
                ImageTapped(sender, e);
        }

        //点击收藏
        private void MarkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (top._id != 0)
                {
                    Data.Message.ShowMessage("已经收藏过");
                    return;
                }
                if (top != null)
                {
                    Data.Database.Insert(top);
                    OnMarkSuccess();
                    Data.Message.ShowMessage("收藏成功");
                }
            }
            catch (Exception ex)
            {
                Data.Message.ShowMessage(ex.Message);
            }
        }

        private void OnMarkSuccess()
        {
            if (this.MarkSuccess != null)
                this.MarkSuccess(this, top);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (MenuClick != null) MenuClick(sender, e);
        }
    }
}
