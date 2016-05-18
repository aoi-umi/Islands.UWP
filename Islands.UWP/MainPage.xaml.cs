using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Islands.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;                
            }

            MainControl mainControlA = new MainControl() {
            Host= Config.A.Host,
            PictureHost = Config.A.PictureHost,
            GetThreadAPI= Config.A.GetThreadAPI,
            GetReplyAPI = Config.A.GetReplyAPI,
            GetRefAPI = Config.A.GetRefAPI,
            PostThreadAPI = Config.A.PostThreadAPI,
            PostReplyAPI = Config.A.PostReplyAPI,
            PageSize = Config.A.PageSize,
            IslandCode = IslandsCode.A
            };
            pivotItemA.Content = mainControlA;


            MainControl mainControlK = new MainControl()
            {
                Host = Config.K.Host,
                PictureHost = Config.K.PictureHost,
                GetThreadAPI = Config.K.GetThreadAPI,
                GetReplyAPI = Config.K.GetReplyAPI,
                GetRefAPI = Config.K.GetRefAPI,
                PostThreadAPI = Config.K.PostThreadAPI,
                PostReplyAPI = Config.K.PostReplyAPI,
                PageSize = Config.K.PageSize,
                IslandCode = IslandsCode.Koukuko
            };
            pivotItemK.Content = mainControlK;

            MainControl mainControlB = new MainControl()
            {
                Host = Config.B.Host,
                PictureHost = Config.B.PictureHost,
                GetThreadAPI = Config.B.GetThreadAPI,
                GetReplyAPI = Config.B.GetReplyAPI,
                GetRefAPI = Config.B.GetRefAPI,
                PostThreadAPI = Config.B.PostThreadAPI,
                PostReplyAPI = Config.B.PostReplyAPI,
                PageSize = Config.B.PageSize,
                IslandCode = IslandsCode.Beitai
            };
            pivotItemB.Content = mainControlB;
        }
    }
}
