using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MainControl : UserControl
    {
        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public IslandsCode IslandCode;

        ThreadsView ThreadControl;
        public MainControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadControl = new ThreadsView()
            {
                postModel = new ThreadsView.PostModel
                {
                    Host = Host,
                    GetThreadAPI = GetThreadAPI,
                    ThreadID = "4"
                },
                islandCode = IslandCode
            };
            mainSplitView.Content = ThreadControl;
        }

        

        private void mainNavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tapped_item = sender as ListBoxItem;
            if (tapped_item != null && tapped_item.Tag != null && tapped_item.Tag.ToString().Equals("0")) 
            {
                mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            }
        }
    }
}
