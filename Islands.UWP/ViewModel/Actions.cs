using Islands.UWP.Model;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                    CurrentControl?.OnSendTapped();
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
    }
}
