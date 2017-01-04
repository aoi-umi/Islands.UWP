using Islands.UWP.Data;
using Islands.UWP.Model;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Islands.UWP.ViewModel
{
    public class Actions : DependencyObject, IAction
    {
        public ActionTypes ActionType
        {
            get { return (ActionTypes)GetValue(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }
        
        public static readonly DependencyProperty ActionTypeProperty =
            DependencyProperty.Register(nameof(ActionType), typeof(ActionTypes), typeof(Actions), new PropertyMetadata(ActionTypes.None));

        public object Execute(object sender, object parameter)
        {
            var ele = sender as FrameworkElement;
            switch (ActionType)
            {
                case ActionTypes.ImageTapped:
                    if (ele != null)
                        ImageTapped(ele.DataContext as ItemViewModel);
                    break;
                case ActionTypes.MenuTapped:
                    CurrentControl?.MenuToggle();
                    break;
                case ActionTypes.BottomInfoTapped:
                    CurrentControl?.BottomRefresh();
                    break;
                case ActionTypes.RefreshTapped:
                    if (ele != null)
                        RefreshTapped(ele.DataContext);
                    break;
                case ActionTypes.SendTapped:
                    CurrentControl?.SendTapped();
                    break;
                case ActionTypes.GotoPageTapped:
                    CurrentControl?.ThreadOrReplyGotoPage();
                    break;
                case ActionTypes.BackTapped:
                    CurrentControl?.Back();
                    break;
                case ActionTypes.MarkTapped:
                    CurrentControl.Mark();
                    break;
                case ActionTypes.ItemRightTapped:
                    ItemRightTapped(sender as FrameworkElement, parameter as RightTappedRoutedEventArgs);
                    break;
                case ActionTypes.FlyoutMenuClicked:
                    if (ele != null)
                        FlyoutMenuClicked(ele.DataContext as ItemViewModel);
                    break;
            }
            return true;
        }
        
        private void ImageTapped(ItemViewModel viewModel)
        {
            if (viewModel != null && !string.IsNullOrEmpty(viewModel.ItemImage))
            {                
                CurrentControl?.ShowImage(viewModel.ItemImage);
            }
        }

        private void RefreshTapped(object dataContext)
        {
            var model = dataContext as BaseListView;
            model?.Refresh();
        }

        private void ItemRightTapped(FrameworkElement ele, RightTappedRoutedEventArgs e)
        {
            if (ele == null || !ele.Resources.ContainsKey("ItemMenuFlyout")) return;
            var flyout = ele.Resources["ItemMenuFlyout"] as MenuFlyout;
            flyout.ShowAt(ele, e.GetPosition(ele));
        }

        private void FlyoutMenuClicked(ItemViewModel viewModel)
        {
            try
            {
                if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.ItemNo)) throw new Exception("引用失败");
                if (CurrentControl != null)
                {
                    var str = ">>No." + viewModel.ItemNo + Environment.NewLine;
                    CurrentControl.InsertSendText(str);
                    CurrentControl.SendTapped();
                }
            }
            catch (Exception ex)
            {
                Message.ShowMessage(ex.Message);
            }
        }

        private MainControl CurrentControl
        {
            get
            {
                Frame rootFrame = Window.Current.Content as Frame;
                var mp = rootFrame.Content as MainPage;
                return mp.CurrentControl;
            }
        }
    }

    public enum ActionTypes
    {
        None,
        ImageTapped,
        MenuTapped,
        BottomInfoTapped,
        RefreshTapped,
        SendTapped,
        GotoPageTapped,
        BackTapped,
        MarkTapped,
        ItemRightTapped,
        ItemHolding,
        FlyoutMenuClicked,
    }
}
