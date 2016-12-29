using Islands.UWP.Model;
using Islands.UWP.ViewModel;
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
            var bindingModel = new BindingModel()
            {
                BindingMode = BindingMode.OneWay,
                BindingElement = this,
                Property = MaskOpacityProperty,
                Path = nameof(MaskOpacity),
                Source = MainPage.Global
            };
            Helper.BindingHelper(bindingModel);
            ItemList.Add(BottomInfoItem);
        }

        public PostRequest postReq;
        public int pageSize { get; set; }
        public string currThread { get; set; }      

        public delegate void MarkSuccessEventHandler(Object sender, ThreadModel t);
        public event MarkSuccessEventHandler MarkSuccess;

        public void GetReplyListByID(string threadId)
        {
            currThread = threadId;
            try
            {
                replyId = threadId;
                postReq.ID = threadId;
                Refresh();
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
                BottomInfoItem .Data = value;
            }
        }
        private DataModel BottomInfoItem = new DataModel()
        {
            DataType = DataTypes.BottomInfo,
            Data = "还未看过任何串(つд⊂)",
        };
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
        private ThreadModel top = null;
        private ReplyModel lastReply = null;

        private void ReplyStatusBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            postReq.Page = currPage;
            GetReplyList(postReq, IslandCode);
        }

        public void BottomRefresh()
        {
            postReq.Page = currPage;
            GetReplyList(postReq, IslandCode);
        }

        private void DataLoading()
        {
            IsLoading = true;
            IsHitTestVisible = false;
            ItemList.Remove(BottomInfoItem);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            ItemList.Add(BottomInfoItem);
            IsLoading = false;
            IsHitTestVisible = true;
        }

        protected override void OnRefresh()
        {
            base.OnRefresh();
            Refresh(1);
        }
        private void Refresh(int page)
        {
            lastReply = null;
            IsGetAllReply = false;
            replyCount = 0;
            currPage = page;
            ItemList.Clear();
            try
            {
                postReq.Page = currPage;
                GetReplyList(postReq, IslandCode);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        private async void GetReplyList(PostRequest req, IslandsCode code)
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
                List<ReplyModel> Replys = null;
                top = null;
                Data.Convert.ResStringToThreadAndReplyList(res, code, out top, out Replys);
                top.islandCode = code;

                int _replyCount;
                int.TryParse(top.replyCount, out _replyCount); 
                if (ItemList.Count == 0 && top != null)
                {
                    top.islandCode = code;
                    var model = new DataModel() { DataType = DataTypes.Thread, Data = top };
                    //var tv = new ThreadView() {Thread = top, IsTextSelectionEnabled = true, Background = null };
                    //tv.IsPo = true;
                    ItemList.Add(model);
                }
                replyCount = _replyCount;
                txtReplyCount = _replyCount.ToString();
                if (Replys == null || Replys.Count == 0)
                {
                    IsGetAllReply = true;
                    throw new Exception("已经没有了");
                }

                if ((ItemList.Count - 1) % (pageSize + 1) == 0)
                    ItemList.Add(new DataModel()
                    {
                        DataType = DataTypes.PageInfo,
                        Data = "Page " + req.Page,
                    });

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
                    reply.islandCode = code;
                    //var rv = new ReplyView() { Reply = reply };
                    //if ((code == IslandsCode.Koukuko && reply.uid == top.uid) || (code != IslandsCode.Koukuko && reply.userid == top.userid))
                    //    rv.IsPo = true;
                    ItemList.Add(new DataModel() { DataType = DataTypes.Reply, Data = reply });
                }
                if (Replys.Count < pageSize || (currPage - 1) * pageSize + Replys.Count == replyCount)
                {
                    IsGetAllReply = true;
                    if (Replys.Count == pageSize) ++currPage;
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
                    GetReplyList(postReq, IslandCode);
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
                Refresh(page);
        }

        //点击收藏
        private void MarkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (top._id != 0)
                {
                    throw new Exception("已经收藏过");
                }
                if (top != null)
                {
                    var mark = Data.Database.GetMarkList(top.islandCode, top.id).FirstOrDefault();
                    if (mark != null && mark._id != 0)
                    {
                        throw new Exception("已经收藏过");
                    }
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
    }
}
