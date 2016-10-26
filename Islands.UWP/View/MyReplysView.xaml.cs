﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplysView : BaseListView
    {

        public MyReplysView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.IslandCode = islandCode;
            InitMyReplyList(islandCode);
            DataContext = this;
        }

        public List<Model.SendModel> myReplyList { get; set; }
        
        public void AddMyReply(Model.SendModel sm)
        {
            myReplyList.Insert(0, sm);
            Items.Insert(0, new MyReplyView(sm, IslandCode) { Tag = sm });
            myReplyCount = myReplyList.Count.ToString();
        }

        private string myReplyCount { set {
                Title.Text = "我的回复(" + value + ")";
            } }

        private bool IsCancelButtonVisible
        {
            set
            {
                CancelButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                BackButton.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return CancelButton.Visibility == Visibility.Visible ? true : false;
            }
        }

        private async void InitMyReplyList(IslandsCode islandCode)
        {
            IsLoading = true;
            Items.Clear();
            await Task.Run(() =>
            {
                myReplyList = Data.Database.GetMyReplyList(islandCode);                
            });
            foreach (var myreply in myReplyList)
            {
                Items.Add(new MyReplyView(myreply, islandCode) { Tag = myreply });
            }
            myReplyCount = myReplyList.Count.ToString();
            IsLoading = false;
        }
        protected override void OnItemClick(object sender, ItemClickEventArgs e)
        {
            MyReplyView view = e.ClickedItem as MyReplyView;
            if (view != null)
            {
                if (view != null && SelectionMode != ListViewSelectionMode.Multiple)
                    base.OnItemClick(sender, e);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancelButtonVisible)
            {
                DeleteAsync();
            }
            else
            {
                SelectionMode = ListViewSelectionMode.Multiple;
            }
            IsCancelButtonVisible = !IsCancelButtonVisible;
        }
        
        private async void DeleteAsync()
        {
            int count = 0;
            var idList = SelectedItems.Where(x => (x as MyReplyView) != null).Select(x => (x as MyReplyView).myReply._id).ToList();
            await Task.Run(() => {
                count = Data.Database.DeleteByIDs(nameof(Model.SendModel), idList);
            });
            Data.Message.ShowMessage($"成功删除{count}项");
            if (count > 0) InitMyReplyList(IslandCode);
            SelectionMode = ListViewSelectionMode.None;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCancelButtonVisible = false;
            SelectionMode = ListViewSelectionMode.None;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitMyReplyList(IslandCode);
        }
    }
}
