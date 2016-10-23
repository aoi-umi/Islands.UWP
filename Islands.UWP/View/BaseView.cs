using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Islands.UWP
{
    public class BaseView : ItemsControl
    {
        private static string ScrollViewerName = "ScrollViewer";
        private static string ListViewName = "ListView";
        private static string ProgressRingName = "ProgressRing";
        private static string MaskName = "Mask";
        public BaseView()
        {
            this.DefaultStyleKey = typeof(BaseView);            
        }

        #region DependencyProperty
        public FrameworkElement TopContent
        {
            get { return (FrameworkElement)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopContentProperty =
            DependencyProperty.Register(nameof(TopContent), typeof(FrameworkElement), typeof(BaseView), new PropertyMetadata(null));

        public FrameworkElement BottomContent
        {
            get { return (FrameworkElement)GetValue(BottomContentProperty); }
            set { SetValue(BottomContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BottomContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BottomContentProperty =
            DependencyProperty.Register(nameof(BottomContent), typeof(FrameworkElement), typeof(BaseView), new PropertyMetadata(null));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(BaseView), new PropertyMetadata(false));

        public double MaskOpacity
        {
            get { return (double)GetValue(MaskOpacityProperty); }
            set { SetValue(MaskOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaskOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaskOpacityProperty =
            DependencyProperty.Register(nameof(MaskOpacity), typeof(double), typeof(BaseView), new PropertyMetadata(0.0));


        #endregion

        private ScrollViewer scrollViewer { get; set; }
        private ListView listView { get; set; }
        private ProgressRing progressRing { get; set; }
        private FrameworkElement MaskView { get; set; }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = GetTemplateChild(ScrollViewerName) as ScrollViewer;
            listView = GetTemplateChild(ListViewName) as ListView;
            progressRing = GetTemplateChild(ProgressRingName) as ProgressRing;
            MaskView = GetTemplateChild(MaskName) as FrameworkElement;

            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            listView.ItemsSource = Items;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = scrollViewer;
            if (sv.VerticalOffset > 0 && sv.ActualHeight + sv.VerticalOffset >= sv.ExtentHeight)
            {
                OnScrollToEnd();
            }
        }

        protected virtual void OnScrollToEnd()
        {
            ScrollViewer sv = scrollViewer;
            //sv.ChangeView(null, sv.VerticalOffset - 1, null);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            OnItemClick(sender, e);
        }

        protected virtual void OnItemClick(object sender, ItemClickEventArgs e)
        {
        }
    }
}
