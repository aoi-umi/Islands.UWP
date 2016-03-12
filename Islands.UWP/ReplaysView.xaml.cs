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
    public sealed partial class ReplysView : UserControl
    {
        public PostModel postModel;
        public IslandsCode islandCode;
        TextBlock ReplyStatusBox = new TextBlock()
        {
            Text = "还未看过任何串(つд⊂)",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        int currPage { get; set; }
        int allPage
        {
            get
            {
                if (replyCount == 0) return 0;
                return replyCount / (pageSize + 1) + 1;
            }
        }
        int replyCount { get; set; }

        string replyId {set { Title.Text= value; } }
        string txtReplyCount { set { ListCount.Text = "(" +value + ")"; } }
        public int pageSize { get; set; }
        public class PostModel
        {
            public string ReplyID { get; set; }
            public string Host { get; set; }
            public string GetReplyAPI { get; set; }
            public PostModel() { }
        }

        private ObservableCollection<Model.ThreadModel> _list = new ObservableCollection<Model.ThreadModel>();
        public ReplysView()
        {
            this.InitializeComponent();
            replyListView.Items.Add(ReplyStatusBox);
            replyListScrollViewer.ViewChanged += ReplyListScrollViewer_ViewChanged;
            ReplyStatusBox.Tapped += ReplyStatusBox_Tapped;
        }

        private void ReplyStatusBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GetReplyList(new Model.PostRequest()
            {
                API = postModel.GetReplyAPI,
                Host = postModel.Host,
                ID = postModel.ReplyID,
                Page = currPage
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
            replyListView.Items.Remove(ReplyStatusBox);
            ReplyStatusBox.Text = "点我加载";
        }
        private void DataLoaded()
        {
            replyListView.Items.Add(ReplyStatusBox);
            Loading.IsActive = false;
            IsHitTestVisible = true;
        }

        public void GetReplyListByID(string replyID)
        {
            try
            {
                replyId = replyID;
                postModel.ReplyID = replyID;
                _Refresh();
            }
            catch (Exception ex)
            {
                ReplyStatusBox.Text = ex.Message;
            }
        }

        private void _Refresh()
        {
            lastReply = null;
            currPage = 1;
            replyListView.Items.Clear();
            try
            {
                GetReplyList(new Model.PostRequest()
                {
                    API = postModel.GetReplyAPI,
                    Host = postModel.Host,
                    ID = postModel.ReplyID,
                    Page = currPage
                }, islandCode);
            }
            catch (Exception ex)
            {
                ReplyStatusBox.Text = ex.Message;
            }
        }

        Model.ReplyModel lastReply = null;
        private async void GetReplyList(Model.PostRequest req, IslandsCode code)
        {
            if (Loading.IsActive) return;
            DataLoading();
            string res = "";
            currPage = req.Page;
            try
            {
                res = await Data.Post.GetData(String.Format(req.API, req.Host, req.ID, req.Page));

                StringReader sr;
                JsonSerializer serializer= new JsonSerializer();
                JObject jObj = (JObject)JsonConvert.DeserializeObject(res);
                Model.ThreadModel top = null;
                JArray Replys = null;
                switch (code)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        sr = new StringReader(res);
                        top = (Model.ThreadModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.ThreadModel));
                        if (jObj["replys"].HasValues)
                            Replys = JArray.Parse(jObj["replys"].ToString());
                        break;
                    case IslandsCode.Koukuko:
                        sr = new StringReader(jObj["threads"].ToString());
                        top = (Model.ThreadModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.ThreadModel));
                        if (jObj["replys"].HasValues)
                            Replys = JArray.Parse(jObj["replys"].ToString());
                        break;
                }
                int _replyCount;
                int.TryParse(top.replyCount, out _replyCount);
                if (replyListView.Items.Count == 0 && top != null)
                {
                    replyListView.Items.Add(new ThreadView(top, code));
                }
                replyCount = _replyCount;
                txtReplyCount = _replyCount.ToString();
                if (Replys == null || Replys.Count == 0)
                    throw new Exception("已经没有了");

                if((replyListView.Items.Count - 1) % (pageSize + 1) == 0)
                    replyListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var reply in Replys)
                {
                    sr = new StringReader(reply.ToString());
                    Model.ReplyModel rm = (Model.ReplyModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.ReplyModel));
                    if (lastReply == null && Replys.IndexOf(reply) == Replys.Count - 1)
                        lastReply = rm;
                    else if (lastReply != null)
                    {
                        switch (code)
                        {
                            case IslandsCode.A:
                            case IslandsCode.Beitai:
                                if (String.Compare(rm.now, lastReply.now) <= 0) continue; break;
                            case IslandsCode.Koukuko:
                                if (String.Compare(rm.createdAt, lastReply.createdAt) <= 0) continue; break;
                        }
                        lastReply = rm;
                    }
                    replyListView.Items.Add(new ReplyView(rm, code));

                }
                if (Replys.Count < pageSize)
                    throw new Exception("已经没有了");
                ++currPage;

            }
            catch (Exception ex)
            {
                ReplyStatusBox.Text = ex.Message;
            }
            finally
            {
                DataLoaded();
            }

        }
        


        //滑动刷新
        private void ReplyListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            if (currPage <= allPage && sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                try
                {
                    sv.ChangeView(null,sv.VerticalOffset - 1,null);
                    GetReplyList(new Model.PostRequest()
                    {
                        API = postModel.GetReplyAPI,
                        Host = postModel.Host,
                        ID = postModel.ReplyID,
                        Page = currPage
                    }, islandCode);
                }
                catch (Exception ex)
                {
                    ReplyStatusBox.Text = ex.Message;
                }
            }
        }

        private void MarkButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
            }         
            catch (Exception ex) {
            }
        }
    }
}
