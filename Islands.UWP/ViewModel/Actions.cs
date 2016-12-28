using Islands.UWP.Model;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Using a DependencyProperty as the backing store for ActionType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionTypeProperty =
            DependencyProperty.Register(nameof(ActionType), typeof(ActionTypes), typeof(Actions), new PropertyMetadata(ActionTypes.None));

        public object Execute(object sender, object parameter)
        {
            var ele = sender as FrameworkElement;
            if (ele != null)
            {
                switch (ActionType)
                {
                    case ActionTypes.ImageTapped:
                        ImageTapped(ele.DataContext);
                        break;
                    case ActionTypes.MenuTapped:
                        CurrentControl?.MenuToggle();
                        break;
                    case ActionTypes.BottomInfoTapped:
                        CurrentControl?.BottomRefresh();
                        break;
                }
                
            }
            return true;
        }

        private void ImageTapped(object DataContext)
        {
            var model = DataContext as ItemViewModel;
            if (model != null)
            {
                CurrentControl?.ShowImage(model.ItemImage);
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
    }
}
