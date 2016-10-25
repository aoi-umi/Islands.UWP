using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MarksView : BaseListView
    {
        public MarksView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.islandCode = islandCode;
            this.DataContext = MainPage.Global;
            InitMarkList(islandCode);
        }

        public List<Model.ThreadModel> markList { get; set; }        

        public void AddMark(Model.ThreadModel tm)
        {            
            markList.Insert(0, tm);
            Items.Insert(0, new ThreadView(tm, islandCode) { Tag = tm, NoImage = true });
            markCount = markList.Count.ToString();
        }

        private IslandsCode islandCode { get; set; }

        private string markCount
        {
            set
            {
                Title.Text = "收藏(" + value + ")";
            }
        }

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

        private async void InitMarkList(IslandsCode islandCode)
        {
            IsLoading = true;
            Items.Clear();
            await Task.Run(() =>
            {
                markList = Data.Database.GetMarkList(islandCode);
            });
            foreach (var mark in markList)
            {
                ThreadView t = new ThreadView(mark, islandCode);
                t.NoImage = true;
                Items.Add(t);
            }
            markCount = markList.Count.ToString();
            IsLoading = false;
        }

        protected override void OnItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null && SelectionMode != ListViewSelectionMode.Multiple)
            {
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
            var idList = SelectedItems.Where(x => (x as ThreadView) != null).Select(x => (x as ThreadView).thread._id).ToList();
            //foreach (var item in SelectedItems)
            //{
            //    ThreadView t = item as ThreadView;
            //    if (t != null) idList.Add(t.thread._id);
            //    //if (t != null && Data.Database.Delete(t.thread))
            //    //{ ++count; }
            //}
            await Task.Run(()=> {
                count = Data.Database.DeleteByIDs(nameof(Model.ThreadModel), idList);
            });
            Data.Message.ShowMessage($"成功删除{count}项");
            if (count > 0) InitMarkList(islandCode);
            SelectionMode = ListViewSelectionMode.None;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCancelButtonVisible = false;
            SelectionMode = ListViewSelectionMode.None;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitMarkList(islandCode);
        }
    }
}
