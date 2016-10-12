using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MainControl : UserControl
    {
        public MainControl()
        {
            this.InitializeComponent();
            DataContext = MainPage.Global;
        }
    
        public class Group<T>
        {
            public string GroupName { get; set; }
            public List<T> Models { get; set; }
        }

        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string GetRefAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public int PageSize { get; set; }
        public IslandsCode IslandCode;        
        
        public delegate void SettingTappedEventHandler(object sender, TappedRoutedEventArgs e);
        public SettingTappedEventHandler SettingTapped;

        private ImageView ImageControl;
        private ThreadsView ThreadControl;
        private ReplysView ReplyControl;
        private MarksView MarkControl;
        private SendView SendControl;
        private MyReplysView MyReplysControl;
        private bool IsMain = true;        

        public void Init()
        {
            Model.ForumModel currForum;
            ForumsListInit(IslandCode, out currForum);

            ThreadControl = new ThreadsView()
            {
                postReq = new Model.PostRequest()
                {
                    Host = Host,
                    API = GetThreadAPI,
                    ID = currForum.forumValue
                },
                islandCode = IslandCode,
                currForum = currForum,
                initTitle = currForum.forumName
            };

            ReplyControl = new ReplysView()
            {
                postReq = new Model.PostRequest
                {
                    Host = Host,
                    API = GetReplyAPI
                },
                islandCode = IslandCode,
                pageSize = PageSize
            };

            MarkControl = new MarksView(IslandCode);

            ImageControl = new ImageView();

            SendControl = new SendView()
            {
                postModel = new SendView.PostModel
                {
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
            ThreadControl.MenuClick += Control_MenuClick;

            ReplyControl.SwitchButton.Click += SwitchButton_Click;
            ReplyControl.MarkSuccess += ReplyControl_MarkSuccess;
            ReplyControl.ImageTapped += Control_ImageTapped;
            ReplyControl.SendButton.Click += SendButton_Click;
            ReplyControl.MenuClick += Control_MenuClick;

            MarkControl.MarkClick += MarkControl_MarkClick;
            MarkControl.BackButton.Click += BackButton_Click;

            SendControl.Response += SendControl_Response;
            SendControl.SendClick += SendControl_SendClick;
            SendControl.BackButton.Click += BackButton_Click;

            ImageControl.BackButton.Click += BackButton_Click;

            MyReplysControl.MyReplyClick += MyReplysControl_MyReplyClick;
            MyReplysControl.BackButton.Click += BackButton_Click;

            BackButton.Click += BackButton_Click;

            mainSplitView.Content = ThreadControl;
        }

        private void Control_MenuClick(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = true;
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
                        mainSplitView.IsPaneOpen = false;
                        if (IsMain)
                            mainSplitView.Content = ThreadControl;
                        else
                            mainSplitView.Content = ReplyControl;
                        break;
                    case "mark":
                        mainSplitView.IsPaneOpen = false;
                        mainSplitView.Content = MarkControl;
                        break;
                    case "myreply":
                        mainSplitView.IsPaneOpen = false;
                        mainSplitView.Content = MyReplysControl;
                        break;
                    case "image":
                        mainSplitView.IsPaneOpen = false;
                        mainSplitView.Content = ImageControl;
                        break;
                    case "forums":
                        mainSplitView.IsPaneOpen = false;
                        mainSplitView.Content = ForumList;
                        break;
                    case "gotothread":
                        mainSplitView.IsPaneOpen = false;
                        GotoThread();
                        break;
                    case "setting":
                        mainSplitView.IsPaneOpen = false;
                        if (SettingTapped != null) SettingTapped(sender, e);
                        //mainSplitView.Content = SettingControl;
                        break;
                    default:
                        mainSplitView.IsPaneOpen = false;
                        break;
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
                var myreply = mpv.Tag as Model.SendModel;
                if (myreply != null)
                {
                    if (myreply.isMain && !string.IsNullOrEmpty(myreply.ThreadId))
                    {
                        IsMain = false;
                        BackToHome();
                        ReplyControl.GetReplyListByID(myreply.ThreadId, 0);
                    }
                    else if (!myreply.isMain && !string.IsNullOrEmpty(myreply.sendId))
                    {
                        IsMain = false;
                        BackToHome();
                        ReplyControl.GetReplyListByID(myreply.sendId, 0);
                    }
                    else {
                        Data.Message.ShowMessage("无法跳转到该串");
                    }
                }
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
                Data.Database.Insert(send);
                MyReplysControl.AddMyReply(send);                
            }
            else {
                Data.Message.ShowMessage("发送失败\n可能原因:\n1.没有cookie\n2.网络原因\n3.内容长度超出范围\n4.图片格式不符或过大\n5.其他原因");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsMain)
            {
                SendControl.title = ThreadControl.currForum.forumName;
                SendControl.postModel.Api = PostThreadAPI;
                SendControl.postModel.Id = ThreadControl.currForum.forumValue;
                if (IslandCode == IslandsCode.Koukuko) SendControl.postModel.Id += "#" + ThreadControl.currForum.forumGroupId;
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
            var groupName = "";
            Group<Model.ForumModel> group = new Group<Model.ForumModel>();
            List<Group<Model.ForumModel>> groups = new List<Group<Model.ForumModel>>();
            foreach (var forum in forums) {
                var split = forum.Split(',');
                if (split[2] == "group")
                {
                    groupName = split[0];
                    group = new Group<Model.ForumModel>() { GroupName = groupName,Models = new List<Model.ForumModel>() };
                    groups.Add(group);
                    continue;
                }
                var f = new Model.ForumModel() {
                    forumName = split[0],
                    forumValue = split[1],
                    forumGroupId = split[2]
                };
                group.Models.Add(f);
                if (groups.Count == 1 && group.Models.Count == 1)
                    currForum = f;
            }
            forumGroup.Source = groups;
            forumGroup.ItemsPath = new PropertyPath("Models");
            forumZoomInView.ItemsSource = forumGroup.View;
            forumZoomOutView.ItemsSource = forumGroup.View.CollectionGroups;
        }

        private void ForumList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var f = e.ClickedItem as Model.ForumModel;
            if (f != null)
            {
                mainSplitView.Content = ThreadControl;
                mainNavigationList.SelectedIndex = 1;
                ThreadControl.RefreshById(f);
            }
        }

        private async void GotoThread()
        {
            var thread = await Data.Message.GotoThreadYesOrNo();
            if (thread > 0)
            {
                IsMain = false;
                BackToHome();
                ReplyControl.GetReplyListByID(thread.ToString(), 0);
            }
        }
    }
}
