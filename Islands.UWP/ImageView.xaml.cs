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
    public sealed partial class ImageView : UserControl
    {
        string _imgageUrl { get; set; }
        public string imageUrl
        {
            set
            {
                _imgageUrl = value;
                ImageWebView.Navigate(new Uri(value));
            }
        }
        public ImageView()
        {
            this.InitializeComponent();
            SaveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_imgageUrl))
            {
                Data.Message.ShowMessage("链接为空");
            }
            else {
                Data.File.SaveFile(_imgageUrl);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ImageWebView.Refresh();
        }
    }
}
