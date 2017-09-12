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

                //statusBar.BackgroundColor = Colors.Transparent;
                //statusBar.BackgroundOpacity = 0;
                //Frame rootFrame = Window.Current.Content as Frame;
                //if (rootFrame != null)
                //{
                //    rootFrame.Margin = new Thickness(0, -statusBar.OccludedRect.Height, 0, 0);
                //}
            }

            this.InitializeComponent();
            Loaded += MainPage_Loaded;
            DataContext = Global;
            #region init pivot content
            
            foreach(var configIsland in Config.Island)
            {
                var mainControl = new MainControl() {
                    IslandConfig = configIsland.Value,
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

            #endregion
            
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
            SettingSwitch(true);
        }

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
                    if (btn.Content.ToString() != "setting")
                    {
                        content.MenuNavigate(btn.Content.ToString());
                        SettingSwitch(false);
                    }
                    else
                    {
                        SettingSwitch(true);
                    }
                }
            }
        }

        public void Back()
        {
            if (!SettingSwitch(false))
            {
                CurrentControl?.Back();
            }
        }

        private bool SettingSwitch(bool visible)
        {
            var result = false;
            if (visible && SettingControl.Visibility != Visibility.Visible)
            {
                /*MovableMenu.Visibility =*/
                mainPivot.Visibility = Visibility.Collapsed;
                SettingControl.Visibility = Visibility.Visible;
                result = true;
            }
            else if (!visible && SettingControl.Visibility == Visibility.Visible)
            {
                /*MovableMenu.Visibility =*/
                mainPivot.Visibility = Visibility.Visible;
                SettingControl.Visibility = Visibility.Collapsed;
                result = true;
            }
            return result;
        }
    }
    
}
