using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Islands.UWP.ViewModel
{
    public class ItemDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            //public string AppResourceRoot = "ms-appx:///";
            //var s = Path.Combine(AppResourceRoot, "/Themes/Generic.xaml");
            var model = item as DataModel;
            if (model != null)
            {
                ResourceDictionary resources = Application.Current.Resources;
                switch (model.DataType)
                {
                    case DataTypes.Thread:
                        return resources["ThreadDataTemplate"] as DataTemplate;
                    case DataTypes.Reply:
                        return resources["ReplyDataTemplate"] as DataTemplate;
                    case DataTypes.MyReply:
                        return resources["MyReplyDataTemplate"] as DataTemplate;
                    case DataTypes.PageInfo:
                        return resources["PageInfoDataTemplate"] as DataTemplate;
                    case DataTypes.BottomInfo:
                        return resources["BottomInfoDataTemplate"] as DataTemplate;
                }
            }
            return base.SelectTemplateCore(item);
        }
    }
}
