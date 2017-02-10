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
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            //Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundColor = Colors.Black;
                statusBar.BackgroundOpacity = 1;
            }

            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            DataContext = Global;
            #region init pivot content
            
            foreach(var configIsland in Config.Island)
            {
                var mainControl = new MainControl() {
                    IslandConfig = configIsland.Value,
                    //Host = configIsland.Value.Host,
                    //PictureHost = configIsland.Value.PictureHost,
                    //GetThreadAPI = configIsland.Value.GetThreadAPI,
                    //GetReplyAPI = configIsland.Value.GetReplyAPI,
                    //GetRefAPI = configIsland.Value.GetRefAPI,
                    //PostThreadAPI = configIsland.Value.PostThreadAPI,
                    //PostReplyAPI = configIsland.Value.PostReplyAPI,
                    //PageSize = configIsland.Value.PageSize,
                    //IslandCode = configIsland.Value.IslandCode,
                };
                mainControl.Init();
                switch (configIsland.Value.IslandCode)
                {
                    case IslandsCode.A:
                        pivotItemA.Content = mainControl;
                        break;
                    case IslandsCode.Koukuko:
                        pivotItemK.Content = mainControl;
                        break;
                    case IslandsCode.Beitai:
                        pivotItemB.Content = mainControl;
                        break;
                }
                mainControl.SettingTapped += SettingTapped;
            }

            #region 旧方法
            //MainControl mainControlA = new MainControl()
            //{
            //    Host = Config.A.Host,
            //    PictureHost = Config.A.PictureHost,
            //    GetThreadAPI = Config.A.GetThreadAPI,
            //    GetReplyAPI = Config.A.GetReplyAPI,
            //    GetRefAPI = Config.A.GetRefAPI,
            //    PostThreadAPI = Config.A.PostThreadAPI,
            //    PostReplyAPI = Config.A.PostReplyAPI,
            //    PageSize = Config.A.PageSize,
            //    IslandCode = IslandsCode.A
            //};
            //mainControlA.Init();
            //pivotItemA.Content = mainControlA;

            //MainControl mainControlK = new MainControl()
            //{
            //    Host = Config.K.Host,
            //    PictureHost = Config.K.PictureHost,
            //    GetThreadAPI = Config.K.GetThreadAPI,
            //    GetReplyAPI = Config.K.GetReplyAPI,
            //    GetRefAPI = Config.K.GetRefAPI,
            //    PostThreadAPI = Config.K.PostThreadAPI,
            //    PostReplyAPI = Config.K.PostReplyAPI,
            //    PageSize = Config.K.PageSize,
            //    IslandCode = IslandsCode.Koukuko
            //};
            //mainControlK.Init();
            //pivotItemK.Content = mainControlK;

            //MainControl mainControlB = new MainControl()
            //{
            //    Host = Config.B.Host,
            //    PictureHost = Config.B.PictureHost,
            //    GetThreadAPI = Config.B.GetThreadAPI,
            //    GetReplyAPI = Config.B.GetReplyAPI,
            //    GetRefAPI = Config.B.GetRefAPI,
            //    PostThreadAPI = Config.B.PostThreadAPI,
            //    PostReplyAPI = Config.B.PostReplyAPI,
            //    PageSize = Config.B.PageSize,
            //    IslandCode = IslandsCode.Beitai
            //};
            //mainControlB.Init();
            //pivotItemB.Content = mainControlB;

            //mainControlA.SettingTapped += SettingTapped;
            //mainControlK.SettingTapped += SettingTapped;
            //mainControlB.SettingTapped += SettingTapped;
            #endregion
            #endregion


            //SettingControl.BackButtonClicked += BackButton_Clicked;
            SettingControl.NightModelToggled += SettingControl_NightModelToggled;
            SettingControl.BackgroundImagePathChanged += SettingControl_BackgroundImagePathChanged;

            MovableMenu.ItemsTapped += MovableMenu_ItemsTapped;
            InitSetting();
        }

        public MainControl CurrentControl
        {
            get
            {
                var ele = mainPivot.SelectedItem as PivotItem;
                return ele == null ? null : ele.Content as MainControl;                
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(MovableMenu, ActualWidth - MovableMenu.ActualWidth);
            Canvas.SetTop(MovableMenu, ActualHeight / 2);
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
            MovableMenu.Visibility = mainPivot.Visibility = Visibility.Collapsed;
            SettingControl.Visibility = Visibility.Visible;
        }

        //private void BackButton_Clicked(object sender, RoutedEventArgs e)
        //{
        //    MovableMenu.Visibility = mainPivot.Visibility = Visibility.Visible;
        //    SettingControl.Visibility = Visibility.Collapsed;
        //}

        private void SettingControl_NightModelToggled(object sender, RoutedEventArgs e)
        {
            if (SettingControl.NightModelIsOn) RequestedTheme = ElementTheme.Dark;
            else RequestedTheme = ElementTheme.Light;
        }

        private void SettingControl_BackgroundImagePathChanged(object sender, string path)
        {
            SetBackgroundImage(MainPage.Global.BackgroundImagePath);
        }

        private void MovableMenu_ItemsTapped(object sender, TappedRoutedEventArgs e)
        {
            var btn = sender as AppBarButton;
            if (btn != null)
            {
                var selectItem = mainPivot.SelectedItem as PivotItem;
                if (selectItem != null)
                {
                    var content = selectItem.Content as MainControl;
                    content.MenuNavigate(btn.Content.ToString());
                }
            }
        }

        public void Back()
        {
            if (SettingControl.Visibility == Visibility.Visible)
            {
                MovableMenu.Visibility = mainPivot.Visibility = Visibility.Visible;
                SettingControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                CurrentControl?.Back();
            }
        }
    }
    
}
