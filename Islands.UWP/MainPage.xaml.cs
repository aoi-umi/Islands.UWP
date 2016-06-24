using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            //CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            DataContext = Global;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;
            }

            MainControl mainControlA = new MainControl()
            {
                Host = Config.A.Host,
                PictureHost = Config.A.PictureHost,
                GetThreadAPI = Config.A.GetThreadAPI,
                GetReplyAPI = Config.A.GetReplyAPI,
                GetRefAPI = Config.A.GetRefAPI,
                PostThreadAPI = Config.A.PostThreadAPI,
                PostReplyAPI = Config.A.PostReplyAPI,
                PageSize = Config.A.PageSize,
                IslandCode = IslandsCode.A
            };
            mainControlA.Init();
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
            mainControlK.Init();
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
            mainControlB.Init();
            pivotItemB.Content = mainControlB;

            mainControlA.SettingTapped += SettingTapped;
            mainControlK.SettingTapped += SettingTapped;
            mainControlB.SettingTapped += SettingTapped;
            
            SettingControl.BackButtonClicked += BackButton_Clicked;
            SettingControl.NightModelToggled += SettingControl_NightModelToggled;

            InitSetting();
        }

        public static MyGlobal Global = new MyGlobal() { TitleFontSize = 14, ContentFontSize = 12};
        public void SetBackgroundImage(string path)
        {
            if (string.IsNullOrEmpty(path)) BackgroundImage.Source = null;
            else Data.File.SetLocalImage(BackgroundImage, path);
        }

        private void InitSetting()
        {
            if(SettingControl.NightModelIsOn) RequestedTheme = ElementTheme.Dark;
            SetBackgroundImage(MainPage.Global.BackgroundImagePath);
        }

        private void SettingTapped(object sender, TappedRoutedEventArgs e)
        {
            SettingControl.Visibility = Visibility.Visible;
        }

        private void BackButton_Clicked(object sender, RoutedEventArgs e)
        {            
            SettingControl.Visibility = Visibility.Collapsed;
            SetBackgroundImage(MainPage.Global.BackgroundImagePath);
        }

        private void SettingControl_NightModelToggled(object sender, RoutedEventArgs e)
        {
            if (SettingControl.NightModelIsOn) RequestedTheme = ElementTheme.Dark;
            else RequestedTheme = ElementTheme.Light;
        }
    }
    
}
