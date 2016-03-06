using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadsView : UserControl
    {
        public bool IsInitRefresh = false;
        public PostModel postModel = new PostModel();
        public IslandCode islandCode;
        int currPage { get; set; }
        public class PostModel
        {
            public string ThreadID { get; set; }
            public string Host { get; set; }
            public string GetThreadAPI { get; set; }
            public PostModel() { }
        }

        private ObservableCollection<Model.AThreadModel> _list = new ObservableCollection<Model.AThreadModel>();

        public ThreadsView()
        {
            this.InitializeComponent();
            threadListScrollViewer.ViewChanged += ThreadListScrollViewer_ViewChanged;
            if (IsInitRefresh)
                _Refresh();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _Refresh();
        }

        private void DataLoading()
        {
            Loading.IsActive = true;
            IsHitTestVisible = false;
        }
        private void DataLoaded()
        {
            Loading.IsActive = false;
            IsHitTestVisible = true;
        }

        private void _Refresh()
        {
            Debug.WriteLine("refresh");
            threadListView.Items.Clear();
            try
            {
                GetThreadList(new Model.PostRequest()
                { 
                    API = postModel.GetThreadAPI,
                    Host = postModel.Host,
                    ID = postModel.ThreadID
                }, islandCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void GetThreadList(Model.PostRequest req, IslandCode code)
        {
            DataLoading();
            string res = "";
            currPage = req.Page;
            try
            {
                res = await Data.Post.GetThreads(String.Format(req.API, req.Host, req.ID, req.Page));
                JArray Threads;
                switch (code) {
                    case IslandCode.A:
                    case IslandCode.Beitai:
                        Threads = JArray.Parse(res);
                        break;
                    case IslandCode.Koukuko:
                        JObject jObj = (JObject)JsonConvert.DeserializeObject(res);
                        Threads = JArray.Parse(jObj["data"]["threads"].ToString());
                        break;
                    default: Threads = new JArray(); break;
                }
                if (Threads.Count == 0)
                    throw new Exception("什么也没有");
                threadListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center});
                foreach (var thread in Threads) {
                    StringReader sr = new StringReader(thread.ToString());
                    JsonSerializer serializer = new JsonSerializer();
                    Model.AThreadModel tm = (Model.AThreadModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.AThreadModel));
                    threadListView.Items.Add(new ThreadView(tm));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                DataLoaded();
            }

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListItemClick();
        }

        protected void ListItemClick()
        {

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
                    sv.ScrollToVerticalOffset(sv.VerticalOffset - 1);
                    GetThreadList(new Model.PostRequest()
                    {
                        API = postModel.GetThreadAPI,
                        Host = postModel.Host,
                        ID = postModel.ThreadID,
                        Page = ++currPage
                    }, islandCode);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}
