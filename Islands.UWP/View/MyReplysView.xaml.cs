using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplysView : BaseView,INotifyPropertyChanged
    {

        public MyReplysView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.islandCode = islandCode;
            InitMyReplyList(islandCode);
            DataContext = this;
        }

        public List<Model.SendModel> myReplyList { get; set; }
        public double TitleFontSize
        {
            get { double value = (double)GetValue(TitleFontSizeProperty); return value == 0 ? 1 : value; }
            set { SetValue(TitleFontSizeProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TitleFontSize")); }
        }
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(double), typeof(MyReplysView), null);
        public double ContentFontSize
        {
            get { double value = (double)GetValue(ContentFontSizeProperty); return value == 0 ? 1 : value; }
            set { SetValue(ContentFontSizeProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ContentFontSize")); }
        }
        public static readonly DependencyProperty ContentFontSizeProperty = DependencyProperty.Register("ContentFontSize", typeof(double), typeof(MyReplysView), null);
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void MyReplyClickEventHandler(Object sender, ItemClickEventArgs e);
        public event MyReplyClickEventHandler MyReplyClick;

        public void AddMyReply(Model.SendModel sm)
        {
            myReplyList.Insert(0, sm);
            Items.Insert(0, new MyReplyView(sm, islandCode) { Tag = sm });
            myReplyCount = myReplyList.Count.ToString();
        }

        private IslandsCode islandCode { get; set; }

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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyReplyView view = e.ClickedItem as MyReplyView;
            if (view != null)
            {
                if (view != null && SelectionMode != ListViewSelectionMode.Multiple)
                    OnItemClick(e);
            }
        }

        private void OnItemClick(ItemClickEventArgs e)
        {
            if (MyReplyClick != null) MyReplyClick(this, e);
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
            var idList = new List<int>();
            foreach (var item in SelectedItems)
            {
                MyReplyView t = item as MyReplyView;
                if (t != null) idList.Add(t.myReply._id);
                //if (t != null && Data.Database.Delete(t.myReply))
                //{ ++count; }
            }
            await Task.Run(() => {
                count = Data.Database.DeleteByIDs(nameof(Model.SendModel), idList);
            });
            Data.Message.ShowMessage($"成功删除{count}项");
            if (count > 0) InitMyReplyList(islandCode);
            SelectionMode = ListViewSelectionMode.Single;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCancelButtonVisible = false;
            SelectionMode = ListViewSelectionMode.Single;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitMyReplyList(islandCode);
        }
    }
}
