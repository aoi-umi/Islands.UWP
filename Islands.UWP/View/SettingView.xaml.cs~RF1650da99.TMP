using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Islands.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            InitSetting();
            BackButton.Click += BackButton_Click;
        }

        private double TitleFontSize
        {
            get { return TitleFontSizeSlider.Value; }
            set { TitleFontSizeSlider.Value = value; }
        }

        private double ContentFontSize
        {
            get { return ContentFontSizeSlider.Value; }
            set { ContentFontSizeSlider.Value = value; }
        }

        private double MaskOpacity
        {
            get { return MaskOpacitySlider.Value; }
            set { MaskOpacitySlider.Value = value; }
        }

        public bool NightModelIsOn
        {
            get { return NightModeSwitch.IsOn; }
            set { NightModeSwitch.IsOn = value; }
        }

        public string ImagePath
        {
            get { return ImagePathBox.Text; }
            set { ImagePathBox.Text = value; }
        }

        public bool IsAskEachTime
        {
            get { return (bool)IsAskEachTimeBox.IsChecked; }
            set { IsAskEachTimeBox.IsChecked = value; }
        }

        public bool IsHideMenu
        {
            get { return IsHideMenuSwitch.IsOn; }
            set { IsHideMenuSwitch.IsOn = value; }
        }

        public bool NoImage
        {
            get { return NoImageSwitch.IsOn; }
            set { NoImageSwitch.IsOn = value; }
        }

        public delegate void NightModeToggledEventHandler(object sender, RoutedEventArgs e);
        public event NightModeToggledEventHandler NightModelToggled;

        public delegate void BackButtonClickedEventHandler(object sender, RoutedEventArgs e);
        public event BackButtonClickedEventHandler BackButtonClicked;

        private IslandsCode islandcode { get; set; }
        private List<Model.SettingModel> SettingList { get; set; }
        private void NightModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (NightModelToggled != null)
            {                
                NightModelToggled(sender, e);
            }
        }        

        private void BackButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (BackButtonClicked != null)
            {
                BackButtonClicked(sender, e);
            }
        }

        private void InitSetting()
        {
            SettingList = Data.Database.GetSettingList(islandcode);
            foreach (Model.SettingModel model in SettingList)
            {
                switch (model.SettingName)
                {
                    case Settings.NightMode:
                        NightModeSwitch.Tag = model;
                        NightModelIsOn = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                    case Settings.TitleFontSize:
                        TitleFontSizeSlider.Tag = model;
                        if (!string.IsNullOrEmpty(model.SettingValue))
                        {
                            double value;
                            Double.TryParse(model.SettingValue, out value);
                            MainPage.Global.TitleFontSize = TitleFontSize = value;
                        }
                        break;
                    case Settings.ContentFontSize:
                        ContentFontSizeSlider.Tag = model;
                        if (!string.IsNullOrEmpty(model.SettingValue))
                        {
                            double value;
                            Double.TryParse(model.SettingValue, out value);
                            MainPage.Global.ContentFontSize = ContentFontSize = value;
                        }
                        break;
                    case Settings.MaskOpacity:
                        MaskOpacitySlider.Tag = model;
                        if (!string.IsNullOrEmpty(model.SettingValue))
                        {
                            double value;
                            Double.TryParse(model.SettingValue, out value);
                            MainPage.Global.MaskOpacity = value / 100;
                            MaskOpacity = value;
                        }
                        break;
                    case Settings.IsAskEachTime:
                        IsAskEachTimeBox.Tag = model;
                        MainPage.Global.IsAskEachTime = IsAskEachTime = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                    case Settings.BackgroundImagePath:
                        BackgroundImage.Tag = model;
                        MainPage.Global.BackgroundImagePath = model.SettingValue;
                        if (!string.IsNullOrEmpty(model.SettingValue)) Data.File.SetLocalImage(BackgroundImage, model.SettingValue);
                        break;
                    case Settings.IsHideMenu:
                        IsHideMenuSwitch.Tag = model;
                        MainPage.Global.IsHideMenu = IsHideMenu = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                    case Settings.NoImage:
                        NoImageSwitch.Tag = model;
                        MainPage.Global.NoImage = NoImage = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            string SettingValue = string.Empty;
            #region 保存夜间模式
            Model.SettingModel model = NightModeSwitch.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.NightMode
            };
            SettingValue = NightModelIsOn ? "on" : string.Empty;

            if (model.SettingValue != SettingValue)
            {
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    NightModeSwitch.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存标题字号
            model = TitleFontSizeSlider.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.TitleFontSize
            };
            SettingValue = TitleFontSize.ToString();

            if (model.SettingValue != SettingValue)
            {
                MainPage.Global.TitleFontSize = TitleFontSize;
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    TitleFontSizeSlider.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存内容字号
            model = ContentFontSizeSlider.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.ContentFontSize
            };
            SettingValue = ContentFontSize.ToString();

            if (model.SettingValue != SettingValue)
            {
                MainPage.Global.ContentFontSize = ContentFontSize;
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    ContentFontSizeSlider.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存遮罩透明度
            model = MaskOpacitySlider.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.MaskOpacity
            };
            SettingValue = MaskOpacity.ToString();

            if (model.SettingValue != SettingValue)
            {
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    MaskOpacitySlider.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存是否每次询问
            model = IsAskEachTimeBox.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.IsAskEachTime
            };
            SettingValue = IsAskEachTime ? "on" : string.Empty;

            if (model.SettingValue != SettingValue)
            {
                MainPage.Global.IsAskEachTime = IsAskEachTime;
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    IsAskEachTimeBox.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 背景图片
            model = BackgroundImage.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.BackgroundImagePath
            };
            SettingValue = MainPage.Global.BackgroundImagePath;

            if (model.SettingValue != SettingValue)
            {
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    BackgroundImage.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存隐藏菜单
            model = IsHideMenuSwitch.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.IsHideMenu
            };
            SettingValue = IsHideMenu ? "on" : string.Empty;

            if (model.SettingValue != SettingValue)
            {
                MainPage.Global.IsHideMenu = IsHideMenu;
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    IsHideMenuSwitch.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion

            #region 保存无图模式
            model = NoImageSwitch.Tag as Model.SettingModel;
            if (model == null) model = new Model.SettingModel()
            {
                SettingName = Settings.NoImage
            };
            SettingValue = NoImage ? "on" : string.Empty;

            if (model.SettingValue != SettingValue)
            {
                MainPage.Global.NoImage = NoImage;
                model.SettingValue = SettingValue;
                if (model._id == 0)
                {
                    Data.Database.Insert(model);
                    NoImageSwitch.Tag = model;
                }
                else Data.Database.Update(model);
            }
            #endregion
            BackButton_Clicked(sender, e);
        }      

        private void MaskOpacitySlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            MainPage.Global.MaskOpacity = MaskOpacitySlider.Value / 100;
        }

        private async void SetImagePathButton_Click(object sender, RoutedEventArgs e)
        {
            //ImagePath = await Data.File.SetPath();
            MainPage.Global.BackgroundImagePath = await Data.File.SetLocalImage(BackgroundImage);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundImage.Source = null;
            MainPage.Global.BackgroundImagePath = null;
        }
    }
}
