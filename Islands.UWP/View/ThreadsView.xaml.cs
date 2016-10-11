using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadsView : UserControl
    {
        public ThreadsView()
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            string DeviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;//Windows.Desktop Windows.Mobile
            threadListScrollViewer.ViewChanged += ThreadListScrollViewer_ViewChanged;
            ThreadStatusBox.Tapped += ThreadStatusBox_Tapped;
            threadListView.Items.Add(ThreadStatusBox);
            currPage = 1;
            if (IsInitRefresh)
                _Refresh(1);
        }

        public bool IsInitRefresh = false;
        public IslandsCode islandCode;
        public Model.PostRequest postReq;
        
        public string initTitle { set { Title.Text = value; } }
        public Model.ForumModel currForum { get; set; }
        
        public delegate void ThreadClickEventHandler(Object sender, ItemClickEventArgs e);
        public event ThreadClickEventHandler ThreadClick;
        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;
        public delegate void MenuClickEventHandler(Object sender, RoutedEventArgs e);
        public event MenuClickEventHandler MenuClick;

        public void RefreshById(Model.ForumModel forum)
        {
            currForum = forum;
            postReq.ID = forum.forumValue;
            _Title = forum.forumName;
            _Refresh(1);
        }

        private TextBlock ThreadStatusBox = new TextBlock()
        {
            Text = "什么也没有(つд⊂),点我加载",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        private string _Title { set { Title.Text = value; } }
        private int currPage { get; set; }
        private string message { set { ThreadStatusBox.Text = value; } }

        private void ThreadStatusBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            postReq.Page = currPage;
            GetThreadList(postReq, islandCode);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _Refresh(1);
        }

        private void DataLoading()
        {
            
            ThreadLoading.IsActive = true;
            IsHitTestVisible = false;
            threadListView.Items.Remove(ThreadStatusBox);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            threadListView.Items.Add(ThreadStatusBox);
            ThreadLoading.IsActive = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            currPage = page;     
            try
            {                
                for (var i = threadListView.Items.Count - 1; i >= 0; i--)
                {
                    threadListView.Items.RemoveAt(i);
                }
                postReq.Page = page;
                GetThreadList(postReq, islandCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        //滑动刷新
        private void ThreadListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            //Debug.WriteLine("{0}|{1}+{2}={3} => {4}", sv.ActualHeight, sv.ViewportHeight, sv.VerticalOffset, sv.ViewportHeight + sv.VerticalOffset, sv.ExtentHeight);
            if (sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                try
                {
                    sv.ChangeView(null, sv.VerticalOffset - 1, null);
                    postReq.Page = currPage;
                    GetThreadList(postReq, islandCode);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

        private async void GetThreadList(Model.PostRequest req, IslandsCode code)
        {
            if (ThreadLoading.IsActive) return;
            DataLoading();
            string res = "";
            try
            {
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<Model.ThreadResponseModel> Threads = null;
                switch (code)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        JArray ja = null;
                        if (!Data.Json.TryDeserialize(res, out ja)) throw new Exception(res.UnicodeDencode());
                        if (ja != null) {
                            Threads = ja.ToObject<List<Model.ThreadResponseModel>>();
                        }
                        break;
                    case IslandsCode.Koukuko:
                        Model.KThreadQueryResponse kRes = Data.Json.Deserialize<Model.KThreadQueryResponse>(res);
                        if (kRes != null && kRes.data != null) {
                            Threads = kRes.data.threads;
                        }
                        break;
                }
                if (Threads == null || Threads.Count == 0)
                    throw new Exception("什么也没有");
                threadListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var thread in Threads)
                {
                    var tv = new ThreadView(thread, code);
                    tv.ImageTapped += Image_ImageTapped;
                    if (!MainPage.Global.NoImage) tv.ShowImage();
                    threadListView.Items.Add(tv);
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

        private async void GotoPageButton_Click(object sender, RoutedEventArgs e)
        {
            var page = await Data.Message.GotoPageYesOrNo();
            if (page > 0)
                _Refresh(page);
        }

        //点击串
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                if (ThreadClick != null)
                    ThreadClick(sender, e);
            }
        }

        //点击图片
        private void Image_ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ImageTapped != null)
                ImageTapped(sender, e);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (MenuClick != null) MenuClick(sender, e);
        }        
    }   
}
