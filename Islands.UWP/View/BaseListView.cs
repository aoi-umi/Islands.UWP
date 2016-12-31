using Islands.UWP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Islands.UWP
{
    public class BaseListView : ListView
    {
        public BaseListView() : base()
        {
            this.DefaultStyleKey = typeof(BaseListView);
            ItemList = new ObservableCollection<DataModel>();
            ItemsSource = ItemList;
        }

        #region DependencyProperty
        public FrameworkElement TopContent
        {
            get { return (FrameworkElement)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }
        
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(FrameworkElement), typeof(BaseListView), new PropertyMetadata(null));

        public Brush TopContentBackground
        {
            get { return (Brush)GetValue(TopContentBackgroundProperty); }
            set { SetValue(TopContentBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty TopContentBackgroundProperty =
            DependencyProperty.Register(nameof(TopContentBackground), typeof(Brush), typeof(BaseListView), new PropertyMetadata(null));


        public FrameworkElement BottomContent
        {
            get { return (FrameworkElement)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }
        
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(FrameworkElement), typeof(BaseListView), new PropertyMetadata(null));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(BaseListView), new PropertyMetadata(false));

        public double MaskOpacity
        {
            get { return (double)GetValue(MaskOpacityProperty); }
            set { SetValue(MaskOpacityProperty, value); }
        }
        
        public static readonly DependencyProperty MaskOpacityProperty =
            DependencyProperty.Register(nameof(MaskOpacity), typeof(double), typeof(BaseListView), new PropertyMetadata(0.0));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BaseListView), new PropertyMetadata(null));

        #endregion

        public IslandsCode IslandCode { get; set; }
        protected ObservableCollection<DataModel> ItemList { get; set; }
        protected DeviceFamily DeviceFamily { get { return Helper.CurrDeviceFamily; } }

        private static string ScrollViewerName = "ScrollViewer";
        private static string RefreshButtonName = "RefreshButton";
        private ScrollViewer scrollViewer { get; set; }
        private Button refreshButton { get; set; }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = GetTemplateChild(ScrollViewerName) as ScrollViewer;
            refreshButton = GetTemplateChild(RefreshButtonName) as Button;

            refreshButton.DataContext = this;
            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
        }

        protected virtual void OnScrollToEnd()
        {
            //ScrollViewer sv = scrollViewer;
            //sv.ChangeView(null, sv.VerticalOffset - 1, null);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = scrollViewer;
            if (sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                OnScrollToEnd();
            }
        }

        public void Refresh(bool clear = true)
        {
            _Refresh(1, clear);
        }

        public void Refresh(int page, bool clear = true)
        {
            _Refresh(page, clear);
        }

        private void _Refresh(int page, bool clear = true)
        {
            if (IsLoading || page <= 0) return;
            if (clear) ItemList.Clear();
            OnRefresh(page);
        }

        protected virtual void OnRefresh(int page)
        {
        }        

        protected virtual void RefreshStart()
        {
            IsHitTestVisible = false;
            IsLoading = true;
        }

        protected virtual void RefreshEnd()
        {
            IsHitTestVisible = true;
            IsLoading = false;
        }
    }
}
