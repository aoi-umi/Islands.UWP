﻿using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            tm.islandCode = IslandCode;
            ItemList.Insert(0, new DataModel() { DataType = DataTypes.Thread, Data = tm });
            //Items.Insert(0, new ThreadView() {Thread = tm, NoImage = true });
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
            //Items.Clear();
            ItemList.Clear();
            await Task.Run(() =>
            {
                markList = Data.Database.GetMarkList(IslandCode);
            });
            foreach (var mark in markList)
            {
                mark.islandCode = IslandCode;
                //ThreadView t = new ThreadView() { Thread = mark };
                //t.NoImage = true;
                //Items.Add(t);
                ItemList.Add(new DataModel() { DataType = DataTypes.Thread, Data = mark });
            }
            markCount = markList.Count.ToString();
            IsLoading = false;
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
            var idList = SelectedItems.Where(x =>
            {
                var model = (x as DataModel);
                if (model != null && (model.Data as ThreadModel) != null) return true;
                return false;
            }).Select(x => ((x as DataModel).Data as ThreadModel)._id).ToList();
            await Task.Run(()=> {
                count = Data.Database.DeleteByIDs(nameof(ThreadModel), idList);
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
