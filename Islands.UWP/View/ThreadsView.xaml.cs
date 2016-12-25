﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadsView : BaseListView
    {
        public ThreadsView()
        {
            InitializeComponent();
            this.DataContext = MainPage.Global;
            ThreadStatusBox.Tapped += ThreadStatusBox_Tapped;
            Items.Add(ThreadStatusBox);
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
        }        
        public Model.PostRequest postReq;
        
        public string initTitle { set { Title.Text = value; } }
        public Model.ForumModel currForum { get; set; }
        
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
            GetThreadList(postReq, IslandCode);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _Refresh(1);
        }

        private void DataLoading()
        {            
            IsLoading = true;
            IsHitTestVisible = false;
            Items.Remove(ThreadStatusBox);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            Items.Add(ThreadStatusBox);
            IsLoading = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            currPage = page;     
            try
            {                
                for (var i = Items.Count - 1; i >= 0; i--)
                {
                    Items.RemoveAt(i);
                }
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

        private async void GetThreadList(Model.PostRequest req, IslandsCode code)
        {
            if (IsLoading) return;
            DataLoading();
            string res = "";
            try
            {
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<Model.ThreadResponseModel> Threads = null;
                Data.Convert.ResStringToThreadList(res, code, out Threads);                
                if (Threads == null || Threads.Count == 0)
                    throw new Exception("什么也没有");
                Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var thread in Threads)
                {
                    thread.islandCode = code;
                    var tv = new ThreadView() { Thread = thread };
                    tv.ImageTapped += Image_ImageTapped;
                    Items.Add(tv);
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
        protected override void OnItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                base.OnItemClick(sender, e);
            }
        }

        //点击图片
        private void Image_ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            ImageTapped?.Invoke(sender, e);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuClick?.Invoke(sender, e);
        }        
    }   
}
