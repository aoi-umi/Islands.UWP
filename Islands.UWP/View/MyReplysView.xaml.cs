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
            DataTypeBox.ItemsSource = Config.DatabaseTypeList;
            DataTypeBox.SelectedIndex = 0;
        }

        //public List<SendModel> myReplyList { get; set; }

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

        private bool Romaing { get { return DataTypeBox.SelectedValue.ToString() == "漫游"; } }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitMyReplyList();
        }

        public void AddMyReply(SendModel sm)
        {
            //if (myReplyList == null) return;
            //myReplyList.Insert(0, sm);
            sm.islandCode = IslandCode;
            ItemList.Insert(0, new DataModel() { DataType = DataTypes.MyReply, Data = sm });
            myReplyCount = ItemList.Count.ToString();// myReplyList.Count.ToString();
        }

        private async void InitMyReplyList()
        {
            RefreshStart();
            var romaing = Romaing;
            List<SendModel> myReplyList = null;
            await Task.Run(() =>
            {
                myReplyList = Data.Database.GetMyReplyList(IslandCode, romaing);
            });
            ItemList.Clear();
            foreach (var myreply in myReplyList)
            {
                myreply.islandCode = IslandCode;
                ItemList.Add(new DataModel() { DataType = DataTypes.MyReply, Data = myreply });
            }
            myReplyCount = ItemList.Count.ToString();// myReplyList.Count.ToString();
            RefreshEnd();
        }

        private async void DeleteAsync()
        {
            var romaing = Romaing;
            int count = 0;
            var idList = SelectedItems.Where(x =>
            {
                var model = (x as DataModel);
                if (model != null && (model.Data as SendModel) != null) return true;
                return false;
            }).Select(x => ((x as DataModel).Data as SendModel)._id).ToList();

            await Task.Run(() =>
            {
                count = Data.Database.DeleteByIDs(nameof(SendModel), idList, romaing);
            });
            if (count > 0) InitMyReplyList();
            SelectionMode = ListViewSelectionMode.None;
            Data.Message.ShowMessage($"成功删除{count}项");
        }

        protected override void OnRefresh(int page)
        {
            base.OnRefresh(page);
            InitMyReplyList();
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsSelectMode = false;
            SelectionMode = ListViewSelectionMode.None;
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != Items.Count)
                SelectAll();
            else
                DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)Items.Count));
        }

        private void DataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnRefresh(1);
        }
    }
}
