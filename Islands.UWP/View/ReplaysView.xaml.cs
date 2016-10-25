using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplysView : BaseListView
    {
        public ReplysView()
        {
            InitializeComponent();
            this.DataContext = MainPage.Global;
            Items.Add(ReplyStatusBox);
            ReplyStatusBox.Tapped += ReplyStatusBox_Tapped;
            var bindingModel = new BindingModel()
            {
                BindingMode = BindingMode.OneWay,
                BindingElement = this,
                Property = MaskOpacityProperty,
                Path = nameof(MaskOpacity),
                Source = MainPage.Global
            };
            Helper.BindingHelper(bindingModel);
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
            IsLoading = true;
            IsHitTestVisible = false;
            Items.Remove(ReplyStatusBox);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            Items.Add(ReplyStatusBox);
            IsLoading = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            lastReply = null;
            IsGetAllReply = false;
            replyCount = 0;
            currPage = page;
            for (var i = Items.Count - 1; i >= 0; i--)
            {
                Items.RemoveAt(i);
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
            if (IsLoading) return;
            txtReplyCount = "0";
            DataLoading();
            string res = "";
            currPage = req.Page;
            try
            {
                if (string.IsNullOrEmpty(req.ID)) throw new Exception("串号为空");
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<Model.ReplyModel> Replys = null;
                top = null;
                Data.Convert.ResStringToThreadAndReplyList(res, code, out top, out Replys);
                top.islandCode = code;
                top._id = markId;

                int _replyCount;
                int.TryParse(top.replyCount, out _replyCount);
                if (Items.Count == 0 && top != null)
                {
                    var tv = new ThreadView(top, code) { Tag = top, IsTextSelectionEnabled = true, Background = null };
                    tv.ImageTapped += Image_ImageTapped;
                    tv.IsPo = true;
                    Items.Add(tv);
                }
                replyCount = _replyCount;
                txtReplyCount = _replyCount.ToString();
                if (Replys == null || Replys.Count == 0)
                {
                    IsGetAllReply = true;
                    throw new Exception("已经没有了");
                }

                if ((Items.Count - 1) % (pageSize + 1) == 0)
                    Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });

                Replys = Replys.OrderBy(x => int.Parse(x.id)).ToList();
                foreach (var reply in Replys)
                {
                    if (lastReply == null && Replys.IndexOf(reply) == Replys.Count - 1)
                        lastReply = reply;
                    else if (lastReply != null)
                    {
                        int currId, lastId;
                        int.TryParse(reply.id, out currId);
                        int.TryParse(lastReply.id, out lastId);
                        if (currId <= lastId) continue;
                        lastReply = reply;
                    }
                    var rv = new ReplyView(reply, code);
                    if ((code == IslandsCode.Koukuko && reply.uid == top.uid) || (code != IslandsCode.Koukuko && reply.userid == top.userid))
                        rv.IsPo = true;
                    rv.ImageTapped += Image_ImageTapped;
                    Items.Add(rv);

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
        protected override void OnScrollToEnd()
        {
            base.OnScrollToEnd();
            if (!IsGetAllReply && currPage <= allPage)
            {
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
            ImageTapped?.Invoke(sender, e);
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
            MarkSuccess?.Invoke(this, top);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuClick?.Invoke(sender, e);
        }
    }
}
