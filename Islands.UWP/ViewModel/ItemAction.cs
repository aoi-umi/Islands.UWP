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
    public class ItemAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var ele = sender as FrameworkElement;
            if (ele != null)
            {
                var model = ele.DataContext as ItemViewModel;
                if (model != null)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    var mp = rootFrame.Content as MainPage;
                    mp.CurrentControl?.ShowImage(model.ItemImage);
                }
            }
            return true;
        }

    }
}
