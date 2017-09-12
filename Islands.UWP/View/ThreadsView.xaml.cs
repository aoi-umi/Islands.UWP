using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadsView : BaseListView
    {
        public ThreadsView() : base()
        {
            InitializeComponent();
            this.DataContext = MainPage.Global;
            currPage = 1;
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
        
        public ForumModel currForum { get; set; }        

        public void RefreshById(ForumModel forum)
        {
            currForum = forum;
            postReq.ID = forum.forumValue;
            Title = forum.forumName;
            Refresh();
        }

        private DataModel BottomInfoItem = new DataModel()
        {
            DataType = DataTypes.BottomInfo,
            Data = "什么也没有(つд⊂),点我加载",
        };
        private int currPage { get; set; }
        private string message { set { BottomInfoItem.Data = value; } }

        public void BottomRefresh()
        {
            postReq.Page = currPage;
            GetThreadList(postReq, IslandCode);
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
            base.OnRefresh(page);
            currPage = page;     
            try
            {
                postReq.Page = page;
                GetThreadList(postReq, IslandCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        //滑动刷新
        protected override void OnScrollToEnd()
        {
            base.OnScrollToEnd();
            try
            {
                postReq.Page = currPage;
                GetThreadList(postReq, IslandCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void GetThreadList(PostRequest req, IslandsCode code)
        {
            if (IsLoading) return;
            req.Host = Config.Island[IslandCode.ToString()].Host;
            RefreshStart();
            string res = "";
            try
            {
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<ThreadResponseModel> Threads;
                Data.Convert.ResStringToThreadList(res, code, out Threads);                
                if (Threads == null || Threads.Count == 0) throw new Exception("什么也没有");
                ItemList.Add(new DataModel() { DataType = DataTypes.PageInfo, Data = "Page " + req.Page });
                foreach (var thread in Threads)
                {
                    thread.islandCode = code;
                    var dataModel = new DataModel() { DataType = DataTypes.Thread, Data = thread };
                    ItemList.Add(dataModel);
                }
                ++currPage;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            RefreshEnd();
        }       
    }   
}
