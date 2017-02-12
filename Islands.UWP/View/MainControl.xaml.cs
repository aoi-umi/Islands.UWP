using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            CurrentContent = CurrentContent;
        }

        public class Group<T>
        {
            public string GroupName { get; set; }
            public List<T> Models { get; set; }
        }

        public IslandConfigModel IslandConfig { get; set; }

        public delegate void SettingTappedEventHandler(object sender, TappedRoutedEventArgs e);
        public SettingTappedEventHandler SettingTapped;

        public UIElement CurrentContent
        {
            get { return mainSplitView.Content; }
            set { mainSplitView.Content = value; }
        }

        private ImageView ImageControl;
        private ThreadsView ThreadControl;
        private ReplysView ReplyControl;
        private MarksView MarkControl;
        private SendView SendControl;
        private MyReplysView MyReplysControl;
        private bool IsMain = true;

        public void Init()
        {
            ForumModel currForum;
            ForumsListInit(IslandConfig.IslandCode, out currForum);

            ThreadControl = new ThreadsView()
            {
                postReq = new PostRequest()
                {
                    Host = IslandConfig.Host,
                    API = IslandConfig.GetThreadAPI,
                    ID = currForum.forumValue
                },
                IslandCode = IslandConfig.IslandCode,
                currForum = currForum,
                Title = currForum.forumName,
            };

            ReplyControl = new ReplysView()
            {
                postReq = new PostRequest
                {
                    Host = IslandConfig.Host,
                    API = IslandConfig.GetReplyAPI
                },
                IslandCode = IslandConfig.IslandCode,
                pageSize = IslandConfig.PageSize,
            };

            MarkControl = new MarksView()
            {
                IslandCode = IslandConfig.IslandCode,
            };

            ImageControl = new ImageView();

            SendControl = new SendView()
            {
                postModel = new SendView.PostModel
                {
                    islandCode = IslandConfig.IslandCode,
                    Host = IslandConfig.Host,
                    Cookie = new CookieModel()
                },
                islandCode = IslandConfig.IslandCode
            };

            MyReplysControl = new MyReplysView()
            {
                IslandCode = IslandConfig.IslandCode,
            };

            AddEvent();
            
            CurrentContent = ThreadControl;
        }

        private void AddEvent()
        {
            ThreadControl.ItemClick += ThreadControl_ThreadClick;

            ReplyControl.MarkSuccess += ReplyControl_MarkSuccess;

            MarkControl.ItemClick += MarkControl_ItemClick;

            SendControl.Response += SendControl_Response;
            SendControl.SendClick += SendControl_SendClick;

            MyReplysControl.ItemClick += MyReplysControl_ItemClick;
        }

        public void MenuToggle()
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null)
            {
                MenuNavigate(tapped_item.Tag.ToString());
            }
        }

        public void MenuNavigate(string type)
        {
            if(type != "menu")
                mainSplitView.IsPaneOpen = false;
            switch (type)
            {
                case "menu":
                    mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
                    break;
                case "home":
                    if (IsMain)
                        CurrentContent = ThreadControl;
                    else
                        CurrentContent = ReplyControl;
                    break;
                case "mark":
                    CurrentContent = MarkControl;
                    break;
                case "myreply":
                    CurrentContent = MyReplysControl;
                    break;
                case "image":
                    CurrentContent = ImageControl;
                    break;
                case "forums":
                    CurrentContent = ForumListView;
                    break;
                case "gotothread":
                    GotoThread();
                    break;
                case "setting":
                    SettingTapped?.Invoke(null, null);
                    break;
            }
        }

        public void ShowImage(string imgPath)
        {
            ImageControl.ImageUrl = imgPath;
            mainNavigationList.SelectedIndex = 5;
            CurrentContent = ImageControl;
        }

        public async void ThreadOrReplyGotoPage()
        {
            var page = await Data.Message.GotoPageYesOrNo();
            if (IsMain)
            {
                ThreadControl.Refresh(page);
            }
            else
            {
                ReplyControl.Refresh(page);
            }
        }

        public void BottomRefresh()
        {
            if (IsMain) ThreadControl.BottomRefresh();
            else ReplyControl.BottomRefresh();
        }

        public void Back()
        {
            if (CurrentContent == ThreadControl || CurrentContent == ReplyControl)
            {
                Switch();
            }
            else
            {
                BackToHome();
            }
        }

        public void SendTapped()
        {
            if (CurrentContent != SendControl)
            {
                GotoSendView();
            }
        }

        public void InsertSendText(string str)
        {
            SendControl.InsertText(str);
        }

        public void Mark()
        {
            ReplyControl.Mark();
        }

        #region private

        private void Switch()
        {
            if (IsMain) CurrentContent = ReplyControl;
            else CurrentContent = ThreadControl;
            IsMain = !IsMain;
        }

        private void BackToHome()
        {
            if (IsMain) CurrentContent = ThreadControl;
            else CurrentContent = ReplyControl;
            mainNavigationList.SelectedIndex = 1;
        }

        private void GotoSendView()
        {
            if (IsMain)
            {
                SendControl.title = ThreadControl.currForum.forumName;
                SendControl.postModel.Api = IslandConfig.PostThreadAPI;
                SendControl.postModel.Id = ThreadControl.currForum.forumValue;
                if (IslandConfig.IslandCode == IslandsCode.Koukuko) SendControl.postModel.Id += "#" + ThreadControl.currForum.forumGroupId;
            }
            else
            {
                SendControl.title = ReplyControl.currThread;
                SendControl.postModel.Api = IslandConfig.PostReplyAPI;
                SendControl.postModel.Id = ReplyControl.currThread;
            }
            SendControl.postModel.IsMain = IsMain;
            CurrentContent = SendControl;
        }

        private void ThreadControl_ThreadClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem as DataModel;
            if (model != null && model.DataType == DataTypes.Thread)
            {
                var item = model.Data as BaseItemModel;
                if (item != null)
                {
                    IsMain = false;
                    CurrentContent = ReplyControl;
                    ReplyControl.GetReplyListByID(item.id);
                }
            }
        }

        private void ReplyControl_MarkSuccess(object sender, ThreadModel t)
        {
            MarkControl.AddMark(t);
        }

        private void MarkControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MarkControl.SelectionMode != ListViewSelectionMode.None) return;
            var model = e.ClickedItem as DataModel;
            if (model != null && model.DataType == DataTypes.Mark)
            {
                var item = model.Data as BaseItemModel;
                if (item != null)
                {
                    IsMain = false;
                    CurrentContent = ReplyControl;
                    mainNavigationList.SelectedIndex = 1;
                    ReplyControl.GetReplyListByID(item.id);
                }
            }
        }

        private void MyReplysControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MyReplysControl.SelectionMode != ListViewSelectionMode.None) return;
            var model = e.ClickedItem as DataModel;
            if (model != null && model.DataType == DataTypes.MyReply)
            {
                var item = model.Data as SendModel;
                if (item != null)
                {
                    if (item.isMain && !string.IsNullOrEmpty(item.ThreadId))
                    {
                        IsMain = false;
                        BackToHome();
                        ReplyControl.GetReplyListByID(item.ThreadId);
                    }
                    else if (!item.isMain && !string.IsNullOrEmpty(item.sendId))
                    {
                        IsMain = false;
                        BackToHome();
                        ReplyControl.GetReplyListByID(item.sendId);
                    }
                    else
                    {
                        Data.Message.ShowMessage("无法跳转到该串");
                    }
                }
            }
        }

        private void SendControl_Response(bool Success, SendModel send)
        {
            if (Success)
            {
                Data.Message.ShowMessage("发送成功");
                Data.Database.Insert(send);
                MyReplysControl.AddMyReply(send);
            }
            else
            {
                Data.Message.ShowMessage("发送失败\n可能原因:\n1.没有cookie\n2.网络原因\n3.内容长度超出范围\n4.图片格式不符或过大\n5.其他原因");
            }
        }

        private void SendControl_SendClick(object sender, RoutedEventArgs e)
        {
            BackToHome();
        }

        private void ForumsListInit(IslandsCode islandCode, out ForumModel currForum)
        {
            currForum = new ForumModel();
            List<String> forums = Config.Island[islandCode.ToString()].Forums;
            //switch (islandCode)
            //{
            //    case IslandsCode.A:
            //        forums = Config.A.Forums;
            //        break;
            //    case IslandsCode.Koukuko:
            //        forums = Config.K.Forums;
            //        break;
            //    case IslandsCode.Beitai:
            //        forums = Config.B.Forums;
            //        break;
            //}
            var groupName = "";
            Group<ForumModel> group = new Group<ForumModel>();
            List<Group<ForumModel>> groups = new List<Group<ForumModel>>();
            foreach (var forum in forums)
            {
                var split = forum.Split(',');
                if (split[2] == "group")
                {
                    groupName = split[0];
                    group = new Group<ForumModel>() { GroupName = groupName, Models = new List<ForumModel>() };
                    groups.Add(group);
                    continue;
                }
                var f = new ForumModel()
                {
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
            var f = e.ClickedItem as ForumModel;
            if (f != null)
            {
                CurrentContent = ThreadControl;
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
                ReplyControl.GetReplyListByID(thread.ToString());
            }
        }
        #endregion
    }

    public enum MenuType
    {
        menu,
        home,
        mark,
        myreply,
        image,
        forums,
        gotothread,
        setting
    }
}
