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
    public sealed partial class MarksView : UserControl
    {
        public delegate void MarkClickEventHandler(Object sender, ItemClickEventArgs e);
        public event MarkClickEventHandler MarkClick;
        public List<Model.ThreadModel> markList { get; set; }
        IslandsCode islandCode { get; set; }
        string markCount { set {
                Title.Text = "收藏(" + value + ")";
            } }
        public MarksView(IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.islandCode = islandCode;
            InitMarkList(islandCode);
        }

        public void AddMark(Model.ThreadModel tm)
        {
            markList.Insert(0, tm);
            markListView.Items.Insert(0, new ThreadView(tm, islandCode) { Tag = tm });
            markCount = markList.Count.ToString();
        }

        private void InitMarkList(IslandsCode islandCode)
        {
            markList = GetMarkList(islandCode);
            foreach (var remark in markList)
            {
                markListView.Items.Add(new ThreadView(remark, islandCode));
            }
            markCount = markList.Count.ToString();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ThreadView tv = e.ClickedItem as ThreadView;
            if (tv != null)
                OnItemClick(e);
        }

        void OnItemClick(ItemClickEventArgs e)
        {
            if (this.MarkClick != null)
                this.MarkClick(this, e);
        }

        private List<Model.ThreadModel> GetMarkList(IslandsCode islandCode)
        {
            using (var conn = Data.Database.GetDbConnection<Model.ThreadModel>())
            {
                if(conn != null)
                    return (from mark in conn.Table<Model.ThreadModel>()
                        where mark.islandCode == islandCode
                        orderby mark._id descending
                        select mark).ToList();
                return new List<Model.ThreadModel>();
            }
        }
    }
}
