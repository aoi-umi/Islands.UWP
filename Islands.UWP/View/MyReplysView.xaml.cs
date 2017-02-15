using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplysView : BaseListView
    {

        public MyReplysView()
        {
            this.InitializeComponent();
            Title = "我的回复";
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitMyReplyList();
        }

        public List<SendModel> myReplyList { get; set; }

        public void AddMyReply(SendModel sm)
        {
            if (myReplyList == null) return;
            myReplyList.Insert(0, sm);
            sm.islandCode = IslandCode;
            ItemList.Insert(0, new DataModel() { DataType = DataTypes.MyReply, Data = sm });
            myReplyCount = myReplyList.Count.ToString();
        }

        private string myReplyCount
        {
            set
            {
                Title = "我的回复(" + value + ")";
            }
        }

        private bool IsSelectMode
        {
            set
            {
                SelectAllButton.Visibility =
                CancelButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                BackButton.Visibility = !value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return CancelButton.Visibility == Visibility.Visible ? true : false;
            }
        }

        private async void InitMyReplyList()
        {
            RefreshStart();
            await Task.Run(() =>
            {
                myReplyList = Data.Database.GetMyReplyList(IslandCode);
            });
            ItemList.Clear();
            foreach (var myreply in myReplyList)
            {
                myreply.islandCode = IslandCode;
                ItemList.Add(new DataModel() { DataType = DataTypes.MyReply, Data = myreply });
            }
            myReplyCount = myReplyList.Count.ToString();
            RefreshEnd();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsSelectMode)
            {
                DeleteAsync();
            }
            else
            {
                SelectionMode = ListViewSelectionMode.Multiple;
            }
            IsSelectMode = !IsSelectMode;
        }

        private async void DeleteAsync()
        {
            int count = 0;
            var idList = SelectedItems.Where(x =>
            {
                var model = (x as DataModel);
                if (model != null && (model.Data as SendModel) != null) return true;
                return false;
            }).Select(x => ((x as DataModel).Data as SendModel)._id).ToList();

            await Task.Run(() =>
            {
                count = Data.Database.DeleteByIDs(nameof(Model.SendModel), idList);
            });
            if (count > 0) InitMyReplyList();
            SelectionMode = ListViewSelectionMode.None;
            Data.Message.ShowMessage($"成功删除{count}项");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelectMode = false;
            SelectionMode = ListViewSelectionMode.None;
        }

        protected override void OnRefresh(int page)
        {
            base.OnRefresh(page);
            InitMyReplyList();
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != Items.Count)
                SelectAll();
            else
                DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)Items.Count));
        }
    }
}
