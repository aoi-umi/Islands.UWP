﻿using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            ItemList.Add(StatusItem);       
        }

        public PostRequest postReq;
        
        public string initTitle { set { Title.Text = value; } }
        public ForumModel currForum { get; set; }        

        public void RefreshById(ForumModel forum)
        {
            currForum = forum;
            postReq.ID = forum.forumValue;
            _Title = forum.forumName;
            _Refresh(1);
        }

        private DataModel StatusItem = new DataModel()
        {
            DataType = DataTypes.PageInfo,
            Data = "什么也没有(つд⊂),点我加载",
        };
        private string _Title { set { Title.Text = value; } }
        private int currPage { get; set; }
        private string message { set { StatusItem.Data = value; } }

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
            ItemList.Remove(StatusItem);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            ItemList.Add(StatusItem);
            IsLoading = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            currPage = page;     
            try
            {
                ItemList.Clear();
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
            DataLoading();
            string res = "";
            try
            {
                res = await Data.Http.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                List<Model.ThreadResponseModel> Threads = null;
                Data.Convert.ResStringToThreadList(res, code, out Threads);                
                if (Threads == null || Threads.Count == 0)
                    throw new Exception("什么也没有");
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
    }   
}
