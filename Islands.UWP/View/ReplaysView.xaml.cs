using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using UmiAoi.UWP;
using Windows.UI.Xaml.Data;

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
        private int _replyCount { get; set; }
        private int replyCount { get { return _replyCount; } set { _replyCount = value; txtReplyCount = value; } }
        private bool IsGetAllReply = false;
        private int txtReplyCount { set { ListCount.Text = "(" + value + "," + allPage + "P)"; } }
        private ThreadModel top = null;
        private ReplyModel lastReply = null;

        public void GetReplyListByID(string threadId)
        {
            currThread = threadId;
            try
            {
                Title = threadId;
                postReq.ID = threadId;
                Refresh();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        public void BottomRefresh()
        {
            postReq.Page = currPage;
            GetReplyList(postReq, IslandCode);
        }

        //收藏
        public void Mark()
        {
            try
            {
                if (top == null) throw new Exception("无法收藏");
                if (top._id != 0) throw new Exception("已经收藏过");
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

        protected override void RefreshStart()
        {
            base.RefreshStart();
            ItemList.Remove(BottomInfoItem);
            message = "点我加载";
        }

        protected override void RefreshEnd()
        {
            base.RefreshEnd();
            ItemList.Add(BottomInfoItem);
        }

        protected override void OnRefresh(int page)
        {
            lastReply = null;
            IsGetAllReply = false;
            replyCount = 0;
            currPage = page;
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
            req.Host = Config.Island[IslandCode.ToString()].Host;
            RefreshStart();
            replyCount = 0;
            string res = "";
            try
            {
                if (string.IsNullOrEmpty(req.ID)) throw new Exception("串号为空");
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<ReplyModel> Replys;
                top = null;
                Data.Convert.ResStringToThreadAndReplyList(res, code, out top, out Replys);
                top.islandCode = code;

                #region top
                int count;
                int.TryParse(top.replyCount, out count);
                if (ItemList.Count == 0)
                {
                    top.islandCode = code;
                    var model = new DataModel()
                    {
                        DataType = DataTypes.Thread,
                        Data = top,
                        Parameter = new ItemParameter() { IsPo = true, IsTextSelectionEnabled = true }
                    };
                    ItemList.Add(model);
                }
                replyCount = count;
                #endregion

                #region check is had reply
                if (Replys == null || Replys.Count == 0)
                {
                    IsGetAllReply = true;
                    throw new Exception("已经没有了");
                }
                #endregion

                #region page info
                if ((ItemList.Count - 1) % (pageSize + 1) == 0)
                {
                    ItemList.Add(new DataModel()
                    {
                        DataType = DataTypes.PageInfo,
                        Data = "Page " + req.Page,
                    });
                }
                #endregion

                #region add item to list
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

                    var dataModel = new DataModel()
                    {
                        DataType = DataTypes.Reply,
                        Data = reply,
                        Parameter = new ItemParameter()
                        {
                            IsTextSelectionEnabled = true,
                            ParentList = ItemList.ToList()
                        }
                    };
                    if ((code == IslandsCode.Koukuko && reply.uid == top.uid) || (code != IslandsCode.Koukuko && reply.userid == top.userid))
                        (dataModel.Parameter as ItemParameter).IsPo = true;
                    ItemList.Add(dataModel);
                }
                #endregion

                #region check is got all
                if (Replys.Count < pageSize || (currPage - 1) * pageSize + Replys.Count == replyCount)
                {
                    IsGetAllReply = true;
                    if (Replys.Count == pageSize) ++currPage;
                    throw new Exception("已经没有了");
                }
                #endregion

                ++currPage;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            RefreshEnd();
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

        private void OnMarkSuccess()
        {
            MarkSuccess?.Invoke(this, top);
        }
    }
}
