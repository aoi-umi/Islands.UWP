using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MainControl : UserControl
    {
        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public int PageSize { get; set; }
        public IslandsCode IslandCode;

        ThreadsView ThreadControl;
        ReplysView ReplyControl;
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
            ThreadControl.ThreadClick += ThreadControl_ThreadClick;
            ThreadControl.SwitchButton.Click += SwitchButton_Click;
            ReplyControl.SwitchButton.Click += SwitchButton_Click;
            mainSplitView.Content = ThreadControl;
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMain) mainSplitView.Content = ReplyControl;
            else mainSplitView.Content = ThreadControl;
            IsMain = !IsMain;
        }

        private void ThreadControl_ThreadClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                IsMain = !IsMain;
                mainSplitView.Content = ReplyControl;
                ReplyControl.GetReplyListByID(tv.threadNo);
            }
        }

        private void mainNavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                    case "forums":
                        mainSplitView.Content = ForumList;
                        break;
                    default: break;
                }
            }
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
                }
            }
        }
    }
}
