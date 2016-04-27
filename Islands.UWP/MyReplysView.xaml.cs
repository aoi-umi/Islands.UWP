using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplysView : UserControl
    {
        public delegate void MyReplyClickEventHandler(Object sender, ItemClickEventArgs e);
        public event MyReplyClickEventHandler MyReplyClick;
        public List<Model.SendModel> myReplyList { get; set; }
        IslandsCode islandCode { get; set; }
        string myReplyCount { set {
                Title.Text = "我的回复(" + value + ")";
            } }
        public MyReplysView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.islandCode = islandCode;
            InitMyReplyList(islandCode);
        }

        public void AddMyReply(Model.SendModel sm)
        {
            myReplyList.Insert(0, sm);
            myreplyListView.Items.Insert(0, new MyReplyView(sm, islandCode) { Tag = sm });
            myReplyCount = myReplyList.Count.ToString();
        }

        private void InitMyReplyList(IslandsCode islandCode)
        {
            myReplyList = GetMyReplyList(islandCode);
            foreach (var myreply in myReplyList)
            {
                myreplyListView.Items.Add(new MyReplyView(myreply, islandCode) { Tag = myreply });
            }
            myReplyCount = myReplyList.Count.ToString();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyReplyView mpv = e.ClickedItem as MyReplyView;
            if (mpv != null)
                OnItemClick(e);
        }

        void OnItemClick(ItemClickEventArgs e)
        {
            if (this.MyReplyClick != null)
                this.MyReplyClick(this, e);
        }

        private List<Model.SendModel> GetMyReplyList(IslandsCode islandCode)
        {
            using (var conn = Data.Database.GetDbConnection<Model.SendModel>())
            {
                if(conn != null)
                    return (from send in conn.Table<Model.SendModel>()
                        where send.islandCode == islandCode
                        orderby send._id descending
                        select send).ToList();
                return new List<Model.SendModel>();
            }
        }
    }
}
