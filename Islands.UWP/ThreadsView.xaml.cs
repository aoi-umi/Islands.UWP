﻿using System;
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
        public IslandsCode islandCode;
        public delegate void ThreadClickEventHandler(Object sender, ItemClickEventArgs e);
        public event ThreadClickEventHandler ThreadClick;
        public class PostModel
        {
            public string ThreadID { get; set; }
            public string Host { get; set; }
            public string GetThreadAPI { get; set; }
            public PostModel() { }
        }

        TextBlock ThreadStatusBox = new TextBlock()
        {
            Text = "什么也没有(つд⊂),点我加载",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        public string initTitle { set { Title.Text = value; } }
        string _Title { set { Title.Text = value; } }
        int currPage { get; set; }
        string message { set { ThreadStatusBox.Text = value; } }

        private ObservableCollection<Model.ThreadModel> _list = new ObservableCollection<Model.ThreadModel>();

        public ThreadsView()
        {
            this.InitializeComponent();
            threadListScrollViewer.ViewChanged += ThreadListScrollViewer_ViewChanged;
            ThreadStatusBox.Tapped += ThreadStatusBox_Tapped;
            threadListView.Items.Add(ThreadStatusBox);
            if (IsInitRefresh)
                _Refresh();
        }

        public Model.ForumModel currForum { get; set; }
        public void RefreshById(Model.ForumModel forum) {
            currForum = forum;
            postModel.ThreadID = forum.forumValue;
            _Title = forum.forumName;
            _Refresh();
        }

        private void ThreadStatusBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetThreadList(new Model.PostRequest()
            {
                API = postModel.GetThreadAPI,
                Host = postModel.Host,
                ID = postModel.ThreadID,
                Page = currPage + 1
            }, islandCode);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _Refresh();
        }

        private void DataLoading()
        {
            
            Loading.IsActive = true;
            IsHitTestVisible = false;
            threadListView.Items.Remove(ThreadStatusBox);
            message = "点我加载";
        }
        private void DataLoaded()
        {
            threadListView.Items.Add(ThreadStatusBox);
            Loading.IsActive = false;
            IsHitTestVisible = true;
        }

        private void _Refresh()
        {
            currPage = 1;
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


        public delegate void ImageTappedEventHandler(Object sender, TappedRoutedEventArgs e);
        public event ImageTappedEventHandler ImageTapped;

        void OnTapped(Object sender, TappedRoutedEventArgs e)
        {
            if (ImageTapped != null)
                ImageTapped(sender, e);
        }

        private async void GetThreadList(Model.PostRequest req, IslandsCode code)
        {
            if (Loading.IsActive) return;
            DataLoading();
            string res = "";
            try
            {
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                JArray Threads;
                switch (code) {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        Threads = JArray.Parse(res);
                        break;
                    case IslandsCode.Koukuko:
                        JObject jObj = (JObject)JsonConvert.DeserializeObject(res);
                        Threads = JArray.Parse(jObj["data"]["threads"].ToString());
                        break;
                    default: Threads = new JArray(); break;
                }
                if (Threads.Count == 0)
                    throw new Exception("什么也没有");
                threadListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var thread in Threads) {
                    StringReader sr = new StringReader(thread.ToString());
                    JsonSerializer serializer = new JsonSerializer();
                    Model.ThreadModel tm = (Model.ThreadModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.ThreadModel));
                    var tv = new ThreadView(tm, code);
                    tv.ImageTapped += Tv_ImageTapped;
                    threadListView.Items.Add(tv);
                }
                currPage = req.Page;

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

        private void Tv_ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ImageTapped != null)
                ImageTapped(sender, e);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
                OnItemClick(e);
        }

        void OnItemClick(ItemClickEventArgs e) {
            if (this.ThreadClick != null)
                this.ThreadClick(this, e);
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
                    sv.ChangeView(null,sv.VerticalOffset - 1,null);
                    GetThreadList(new Model.PostRequest()
                    {
                        API = postModel.GetThreadAPI,
                        Host = postModel.Host,
                        ID = postModel.ThreadID,
                        Page = currPage + 1
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
