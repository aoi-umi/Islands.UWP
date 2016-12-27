using Islands.UWP.Model;
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
            ThreadStatusBox.Tapped += ThreadStatusBox_Tapped;
            //Items.Add(ThreadStatusBox);
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
            list = new ObservableCollection<DataModel>();
            ItemsSource = list;
            //ItemsSource = new List<string> { "123", "465848"};
            //list.Add(new ThreadModel() { content = "123" });
        }

        private ObservableCollection<DataModel> list { get; set; }
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

        private TextBlock ThreadStatusBox = new TextBlock()
        {
            Text = "什么也没有(つд⊂),点我加载",
            HorizontalAlignment = HorizontalAlignment.Center,            
            VerticalAlignment = VerticalAlignment.Center
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
            //Items.Remove(ThreadStatusBox);
            message = "点我加载";
        }

        private void DataLoaded()
        {
            //Items.Add(ThreadStatusBox);
            IsLoading = false;
            IsHitTestVisible = true;
        }

        private void _Refresh(int page)
        {
            currPage = page;     
            try
            {
                //for (var i = Items.Count - 1; i >= 0; i--)
                //{
                //    Items.RemoveAt(i);
                //}
                list.Clear();
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
                //Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center,VerticalAlignment = VerticalAlignment.Center });
                foreach (var thread in Threads)
                {
                    thread.islandCode = code;
                    //var tv = new ThreadView() { Thread = thread };
                    //Items.Add(tv);
                    //var x = thread as ThreadModel;
                    var dataModel = new DataModel() { DataType = DataTypes.Thread, Data = thread };
                    list.Add(dataModel);
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
