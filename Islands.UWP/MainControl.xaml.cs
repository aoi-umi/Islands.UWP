﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MainControl : UserControl
    {
        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string GetRefAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public int PageSize { get; set; }
        public IslandsCode IslandCode;

        ImageView ImageControl;
        ThreadsView ThreadControl;
        ReplysView ReplyControl;
        MarksView MarkControl;
        SendView SendControl;
        MyReplysView MyReplysControl;
        bool IsMain = true;
        public MainControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Model.ForumModel currForum;
            ForumsListInit(IslandCode, out currForum);
            ThreadControl = new ThreadsView()
            {
                postModel = new ThreadsView.PostModel
                {
                    Host = Host,
                    GetThreadAPI = GetThreadAPI,
                    ThreadID = currForum.forumValue
                },
                islandCode = IslandCode,
                currForum = currForum,
                initTitle = currForum.forumName
            };

            ReplyControl = new ReplysView()
            {
                postModel = new ReplysView.PostModel
                {
                    Host = Host,
                    GetReplyAPI = GetReplyAPI,
                    ReplyID = "0"
                },
                islandCode = IslandCode,
                pageSize = PageSize
            };

            MarkControl = new MarksView(IslandCode);
            ImageControl = new ImageView();
            SendControl = new SendView() {
                postModel = new SendView.PostModel {
                    islandCode = IslandCode,
                    Host = Host,
                    Cookie = new Model.CookieModel()
                },
                islandCode = IslandCode
            };
            MyReplysControl = new MyReplysView(IslandCode);

            ThreadControl.ThreadClick += ThreadControl_ThreadClick;
            ThreadControl.SwitchButton.Click += SwitchButton_Click;
            ThreadControl.ImageTapped += Control_ImageTapped;
            ThreadControl.SendButton.Click += SendButton_Click;
            ReplyControl.SwitchButton.Click += SwitchButton_Click;
            ReplyControl.MarkSuccess += ReplyControl_MarkSuccess;
            ReplyControl.ImageTapped += Control_ImageTapped;
            ReplyControl.SendButton.Click += SendButton_Click;
            MarkControl.MarkClick += MarkControl_MarkClick;
            SendControl.Response += SendControl_Response;
            SendControl.SendClick += SendControl_SendClick;
            ImageControl.BackButton.Click += BackButton_Click;
            MyReplysControl.MyReplyClick += MyReplysControl_MyReplyClick;

            mainSplitView.Content = ThreadControl;
        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null)
            {
                switch (tapped_item.Tag.ToString())
                {
                    case "menu":
                        mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
                        break;
                    case "home":
                        if (IsMain)
                            mainSplitView.Content = ThreadControl;
                        else
                            mainSplitView.Content = ReplyControl;
                        break;
                    case "mark":
                        mainSplitView.Content = MarkControl;
                        break;
                    case "myreply":
                        mainSplitView.Content = MyReplysControl;
                        break;
                    case "image":
                        mainSplitView.Content = ImageControl;
                        break;
                    case "forums":
                        mainSplitView.Content = ForumList;
                        break;
                    default: break;
                }
            }
        }

        private void ThreadControl_ThreadClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                IsMain = false;
                mainSplitView.Content = ReplyControl;
                var markId = (from mark in MarkControl.markList
                              where mark.id == tv.threadNo
                              select mark._id
                              ).FirstOrDefault();
                ReplyControl.GetReplyListByID(tv.threadNo, markId);
            }
        }

        private void ReplyControl_MarkSuccess(object sender, Model.ThreadModel t)
        {
            MarkControl.AddMark(t);
        }

        private void Control_ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            var image = sender as Image;
            if (image != null)
            {
                ImageControl.imageUrl = image.Tag.ToString();
                mainNavigationList.SelectedIndex = 5;
                mainSplitView.Content = ImageControl;
            }
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMain) mainSplitView.Content = ReplyControl;
            else mainSplitView.Content = ThreadControl;
            IsMain = !IsMain;
        }

        private void MarkControl_MarkClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                IsMain = false;
                mainSplitView.Content = ReplyControl;
                mainNavigationList.SelectedIndex = 1;
                var tm = tv.Tag as Model.ThreadModel;
                var id = 1;
                if (tm != null)
                    id = tm._id;
                ReplyControl.GetReplyListByID(tv.threadNo, id);
            }
        }

        private void MyReplysControl_MyReplyClick(object sender, ItemClickEventArgs e)
        {
            MyReplyView mpv = e.ClickedItem as MyReplyView;
            if (mpv != null)
            {
                IsMain = false;
                mainSplitView.Content = ReplyControl;
                mainNavigationList.SelectedIndex = 1;
                var myreply = mpv.Tag as Model.SendModel;
                if (myreply != null)
                    if (myreply.isMain && myreply.islandCode == IslandsCode.Koukuko) ReplyControl.GetReplyListByID(myreply.ThreadId, 0);
                    else if (!myreply.isMain) ReplyControl.GetReplyListByID(myreply.sendId, 0);
            }
        }

        private void SendControl_SendClick(object sender, RoutedEventArgs e)
        {
            BackToHome();
        }

        private void SendControl_Response(bool Success, Model.SendModel send)
        {
            if (Success)
            {
                Data.Message.ShowMessage("发送成功");
                using (var conn = Data.Database.GetDbConnection<Model.SendModel>())
                {
                    if (conn != null)
                    {
                        conn.Insert(send);
                        MyReplysControl.AddMyReply(send);
                    }
                }
            }
            else {
                Data.Message.ShowMessage("发送失败");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMain)
            {
                SendControl.title = ThreadControl.currForum.forumName;
                SendControl.postModel.Api = PostThreadAPI;
                SendControl.postModel.Id = ThreadControl.currForum.forumValue;
            }
            else {
                SendControl.title = ReplyControl.currThread;
                SendControl.postModel.Api = PostReplyAPI;
                SendControl.postModel.Id = ReplyControl.currThread;
            }
            SendControl.postModel.IsMain = IsMain;
            mainSplitView.Content = SendControl;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackToHome();
        }

        private void BackToHome()
        {
            if (IsMain) mainSplitView.Content = ThreadControl;
            else mainSplitView.Content = ReplyControl;
            mainNavigationList.SelectedIndex = 1;
        }

        private void ForumsListInit(IslandsCode islandCode, out Model.ForumModel currForum)
        {
            currForum = new Model.ForumModel();
            List<String> forums = null;
            switch (islandCode)
            {
                case IslandsCode.A:
                    forums = Config.A.Forums;
                    break;
                case IslandsCode.Koukuko:
                    forums = Config.K.Forums;
                    break;
                case IslandsCode.Beitai:
                    forums = Config.B.Forums;
                    break;
            }
            foreach (var forum in forums) {
                var split = forum.Split(',');
                if (string.IsNullOrEmpty(split[1]))
                    continue;
                var f = new Model.ForumModel() {
                    forumName = split[0],
                    forumValue = split[1]
                };
                if (ForumList.Items.Count == 0)
                    currForum = f;
                ForumList.Items.Add(new TextBlock()
                {
                    Text = split[0],
                    Tag = f
                });
            }
        }

        private void ForumList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tb = e.ClickedItem as TextBlock;
            if (e != null) {
                var f = tb.Tag as Model.ForumModel;
                if(f != null)
                {
                    ThreadControl.RefreshById(f);
                    mainSplitView.Content = ThreadControl;
                    mainNavigationList.SelectedIndex = 1;
                }
            }
        }
    }
}
