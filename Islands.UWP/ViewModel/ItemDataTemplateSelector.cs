﻿using Islands.UWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    case DataTypes.PageInfo:
                        return resources["PageInfoDataTemplate"] as DataTemplate;
                }
            }
            return base.SelectTemplateCore(item);
        }
    }
}
