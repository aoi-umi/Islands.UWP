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
        public MarksView()
        {
            this.InitializeComponent();
            this.DataContext = MainPage.Global;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitMarkList();
        }

        public List<Model.ThreadModel> markList { get; set; }        

        public void AddMark(Model.ThreadModel tm)
        {
            if (markList == null) return;
            markList.Insert(0, tm);
            Items.Insert(0, new ThreadView(tm, IslandCode) { Tag = tm, NoImage = true });
            markCount = markList.Count.ToString();
        }

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

        private async void InitMarkList()
        {
            IsLoading = true;
            Items.Clear();
            await Task.Run(() =>
            {
                markList = Data.Database.GetMarkList(IslandCode);
            });
            foreach (var mark in markList)
            {
                ThreadView t = new ThreadView(mark, IslandCode);
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
            await Task.Run(()=> {
                count = Data.Database.DeleteByIDs(nameof(Model.ThreadModel), idList);
            });
            if (count > 0) InitMarkList();
            SelectionMode = ListViewSelectionMode.None;
            Data.Message.ShowMessage($"成功删除{count}项");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCancelButtonVisible = false;
            SelectionMode = ListViewSelectionMode.None;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitMarkList();
        }
    }
}
