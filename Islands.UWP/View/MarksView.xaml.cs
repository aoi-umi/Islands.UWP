using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MarksView : UserControl
    {
        public MarksView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.islandCode = islandCode;
            this.DataContext = MainPage.Global;
            InitMarkList(islandCode);
        }

        public List<Model.ThreadModel> markList { get; set; }

        public delegate void MarkClickEventHandler(Object sender, ItemClickEventArgs e);
        public event MarkClickEventHandler MarkClick;

        public void AddMark(Model.ThreadModel tm)
        {
            markList.Insert(0, tm);
            markListView.Items.Insert(0, new ThreadView(tm, islandCode) { Tag = tm });
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

        private void InitMarkList(IslandsCode islandCode)
        {
            markListView.Items.Clear();
            markList = Data.Database.GetMarkList(islandCode);
            foreach (var mark in markList)
            {
                ThreadView t = new ThreadView(mark, islandCode);
                t.NoImage = true;
                markListView.Items.Add(t);
            }
            markCount = markList.Count.ToString();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
            {
                if (tv.IsCheckboxDisplay) {
                    tv.IsSelected = !tv.IsSelected;
                    if (tv.IsSelected) tv.Background = Config.SelectedColor;
                    else tv.Background = null;                                  
                }
                else OnItemClick(e);
            }
        }

        private void OnItemClick(ItemClickEventArgs e)
        {
            if (MarkClick != null)
                MarkClick(this, e);
        }        

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var item in markListView.Items)
            {
                ThreadView t = item as ThreadView;
                if (t != null)
                {
                    if (!IsCancelButtonVisible) t.IsCheckboxDisplay = true;
                    else {
                        if (t.IsSelected && Data.Database.Delete(t.thread)) { ++count; }
                    }
                }
            }
            if (IsCancelButtonVisible) {                
                Data.Message.ShowMessage($"成功删除{count}项");
                if (count > 0) InitMarkList(islandCode);
                else HideCheckbox();
            }
            IsCancelButtonVisible = !IsCancelButtonVisible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCancelButtonVisible = false;
            HideCheckbox();
        }

        private void HideCheckbox()
        {
            foreach (var item in markListView.Items)
            {
                var view = item as ThreadView;
                if (view != null && view.IsCheckboxDisplay) view.IsCheckboxDisplay = false;
            }
        }
    }
}
