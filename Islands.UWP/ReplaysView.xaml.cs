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
        int pageSize { get; set; }
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
            replyListScrollViewer.ViewChanged += ReplyListScrollViewer_ViewChanged;
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

        public void GetReplyListByID(string replyID)
        {
            try
            {
                postModel.ReplyID = replyID;
                GetReplyList(new Model.PostRequest()
                {
                    API = postModel.GetReplyAPI,
                    Host = postModel.Host,
                    ID = postModel.ReplyID,
                    Page = 1
                }, islandCode);
            }
            catch (Exception ex)
            {

            }
        }

        private void _Refresh()
        {
            replyListView.Items.Clear();
            try
            {
                GetReplyList(new Model.PostRequest()
                {
                    API = postModel.GetReplyAPI,
                    Host = postModel.Host,
                    ID = postModel.ReplyID,
                    Page = 1
                }, islandCode);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void GetReplyList(Model.PostRequest req, IslandsCode code)
        {
            DataLoading();
            string res = "";
            currPage = req.Page;
            try
            {
                res = await Data.Post.GetData(String.Format(req.API, req.Host, req.ID, req.Page));
                JObject jObj = (JObject)JsonConvert.DeserializeObject(res);
                JArray Replys;
                Debug.WriteLine(res);
                switch (code)
                {
                    case IslandsCode.A:
                    case IslandsCode.Beitai:
                        Replys = JArray.Parse(jObj["replys"].ToString());
                        break;
                    case IslandsCode.Koukuko:
                        Replys = JArray.Parse(jObj["data"]["threads"].ToString());
                        break;
                    default: Replys = new JArray(); break;
                }
                if (Replys.Count == 0)
                    throw new Exception("什么也没有");
                replyListView.Items.Add(new TextBlock() { Text = "Page " + req.Page, HorizontalAlignment = HorizontalAlignment.Center });
                foreach (var reply in Replys)
                {
                    StringReader sr = new StringReader(reply.ToString());
                    JsonSerializer serializer = new JsonSerializer();
                    Model.ReplyModel rm = (Model.ReplyModel)serializer.Deserialize(new JsonTextReader(sr), typeof(Model.ReplyModel));
                    rm.islandCode = code;
                    replyListView.Items.Add(new ReplyView(rm));
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
        


        //滑动刷新
        private void ReplyListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)sender;
            //Debug.WriteLine("{0}|{1}+{2}={3} => {4}", sv.ActualHeight, sv.ViewportHeight, sv.VerticalOffset, sv.ViewportHeight + sv.VerticalOffset, sv.ExtentHeight);
            if (sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                try
                {
                    sv.ChangeView(null,sv.VerticalOffset - 1,null);
                    GetReplyList(new Model.PostRequest()
                    {
                        API = postModel.GetReplyAPI,
                        Host = postModel.Host,
                        ID = postModel.ReplyID,
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
