using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ImageView : UserControl
    {
        private string _imgageUrl { get; set; }
        public string imageUrl
        {
            set
            {
                _imgageUrl = value;
                ImageWebView.NavigateToString("<html></html>");
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
                if (MainPage.Global.IsAskEachTime) Data.File.SaveFile(_imgageUrl);
                else Data.File.SaveFileWithoutDialog(_imgageUrl);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ImageWebView.Refresh();
        }
    }
}
