using Islands.UWP.Data;
using Islands.UWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Islands.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingView : BaseContentView
    {
        public SettingView()
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            InitSetting();
            settingUIList = new List<UIElement>() {
                NightModeSwitch,
                TitleFontSizeSlider,
                ContentFontSizeSlider,
                MaskOpacitySlider,
                IsAskEachTimeBox,
                IsHideMenuSwitch,
                NoImageSwitch,
                AHostBox,
                APictureHostBox,
                KHostBox,
                KPictureHostBox,
                BHostBox,
                BPictureHostBox,
            };
            
            Loaded += SettingView_Loaded;
            Unloaded += SettingView_Unloaded;
        }
        private List<UIElement> settingUIList;
        private SettingModel BackgroundImageSettingModel { get; set; }

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

        private double MaskOpacitySliderValue
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

        private IslandsCode islandcode { get; set; }
        private List<SettingModel> SettingList { get; set; }

        public delegate void NightModeToggledEventHandler(object sender, RoutedEventArgs e);
        public event NightModeToggledEventHandler NightModelToggled;

        //public delegate void BackButtonClickedEventHandler(object sender, RoutedEventArgs e);
        //public event BackButtonClickedEventHandler BackButtonClicked;

        public delegate void BackgroundImagePathChangedEventHandler(object sender, string path);
        public event BackgroundImagePathChangedEventHandler BackgroundImagePathChanged;

        #region event
        private void SettingView_Loaded(object sender, RoutedEventArgs e)
        {
            AddEvent();
        }

        private void SettingView_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveEvent();
        }

        private void NightModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            NightModelToggled?.Invoke(sender, e);
        }

        private async void Settings_LostFocus(object sender, RoutedEventArgs e)
        {
            var ele = sender as FrameworkElement;
            var model = ele.Tag as SettingModel;
            string SettingName = string.Empty;
            string SettingValue = string.Empty;
            if (model == null)
            {
                model = new SettingModel() { SettingValue = string.Empty };
                ele.Tag = model;
            }
            switch (ele.Name)
            {
                #region 夜间模式
                case "NightModeSwitch":
                    SettingName = Settings.NightMode;
                    SettingValue = NightModelIsOn ? "on" : string.Empty;
                    break;
                #endregion

                #region 标题字号
                case "TitleFontSizeSlider":
                    SettingName = Settings.TitleFontSize;
                    SettingValue = TitleFontSize.ToString();
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.TitleFontSize = TitleFontSize;
                    break;
                #endregion

                #region 内容字号
                case "ContentFontSizeSlider":
                    SettingName = Settings.ContentFontSize;
                    SettingValue = ContentFontSize.ToString();
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.ContentFontSize = ContentFontSize;
                    break;
                #endregion

                #region 遮罩透明度
                case "MaskOpacitySlider":
                    SettingName = Settings.MaskOpacity;
                    SettingValue = MaskOpacitySliderValue.ToString();
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.MaskOpacity = MaskOpacitySliderValue / 100;
                    break;
                #endregion

                #region 是否每次询问
                case "IsAskEachTimeBox":
                    SettingName = Settings.IsAskEachTime;
                    SettingValue = IsAskEachTime ? "on" : string.Empty;
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.IsAskEachTime = IsAskEachTime;
                    break;
                #endregion

                #region 隐藏菜单
                case "IsHideMenuSwitch":
                    SettingName = Settings.IsHideMenu;
                    SettingValue = IsHideMenu ? "on" : string.Empty;
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.IsHideMenu = IsHideMenu;
                    break;
                #endregion

                #region 无图模式
                case "NoImageSwitch":
                    SettingName = Settings.NoImage;
                    SettingValue = NoImage ? "on" : string.Empty;
                    if (SettingValue != model.SettingValue)
                        MainPage.Global.NoImage = NoImage;
                    break;
                #endregion

                #region A岛域名
                case "AHostBox":
                    SettingName = Settings.AHost;
                    SettingValue = AHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.A.ToString()].Host)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.A.ToString()].Host = SettingValue;
                    break;
                #endregion
                #region A岛图片域名
                case "APictureHostBox":
                    SettingName = Settings.APictureHost;
                    SettingValue = APictureHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.A.ToString()].PictureHost)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.A.ToString()].PictureHost = SettingValue;
                    break;
                #endregion

                #region 光驱岛域名
                case "KHostBox":
                    SettingName = Settings.KHost;
                    SettingValue = KHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.Koukuko.ToString()].Host)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.Koukuko.ToString()].Host = SettingValue;
                    break;
                #endregion
                #region 光驱岛图片域名
                case "KPictureHostBox":
                    SettingName = Settings.KPictureHost;
                    SettingValue = KPictureHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.Koukuko.ToString()].PictureHost)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.Koukuko.ToString()].PictureHost = SettingValue;
                    break;
                #endregion

                #region 备胎岛域名
                case "BHostBox":
                    SettingName = Settings.BHost;
                    SettingValue = BHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.Beitai.ToString()].Host)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.Beitai.ToString()].Host = SettingValue;
                    break;
                #endregion
                #region 备胎岛图片域名
                case "BPictureHostBox":
                    SettingName = Settings.BPictureHost;
                    SettingValue = BPictureHostBox.Text.Trim();
                    if (SettingValue == Config.Island[IslandsCode.Beitai.ToString()].PictureHost)
                        SettingValue = string.Empty;
                    else
                        Config.Island[IslandsCode.Beitai.ToString()].PictureHost = SettingValue;
                    break;
                    #endregion
            }
            //SettingName不为空,且值与原来不同时保存
            if (!string.IsNullOrWhiteSpace(SettingName) &&
                (SettingName != model.SettingName || SettingValue != model.SettingValue))
            {
                model.SettingName = SettingName;
                model.SettingValue = SettingValue;
                await SaveSettingAsync(model);
            }
        }

        private void MaskOpacitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           // MainPage.Global.MaskOpacity = MaskOpacitySlider.Value / 100;
        }

        private async void SetImagePathButton_Click(object sender, RoutedEventArgs e)
        {
            string path = await Data.File.CopyImageToLocal("Background");
            if (!string.IsNullOrEmpty(path))
            {
                MainPage.Global.BackgroundImagePath = path;
                BackgroundImagePathChange(path);                
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Global.BackgroundImagePath = null;
            BackgroundImagePathChange(null);
        }

        private void DataRoamingButton_Click(object sender, RoutedEventArgs e)
        {
            DataRoamingButton.Visibility = Visibility.Collapsed;
            RoamingDataAsync();
        }
        #endregion

        private void AddEvent()
        {
            DataRoamingButton.Click += DataRoamingButton_Click;
            settingUIList.ForEach(x => { x.LostFocus += Settings_LostFocus; });
        }

        private void RemoveEvent()
        {
            DataRoamingButton.Click -= DataRoamingButton_Click;
            settingUIList.ForEach(x => { x.LostFocus -= Settings_LostFocus; });
        }

        private void InitSetting()
        {
            SettingList = Database.GetSettingList(islandcode);
            foreach (SettingModel model in SettingList)
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
                            MaskOpacitySliderValue = value;
                        }
                        break;
                    case Settings.IsAskEachTime:
                        IsAskEachTimeBox.Tag = model;
                        MainPage.Global.IsAskEachTime = IsAskEachTime = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                    case Settings.BackgroundImagePath:
                        BackgroundImageSettingModel = model;
                        MainPage.Global.BackgroundImagePath = model.SettingValue;
                        break;
                    case Settings.IsHideMenu:
                        IsHideMenuSwitch.Tag = model;
                        MainPage.Global.IsHideMenu = IsHideMenu = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;
                    case Settings.NoImage:
                        NoImageSwitch.Tag = model;
                        MainPage.Global.NoImage = NoImage = string.IsNullOrEmpty(model.SettingValue) ? false : true;
                        break;

                    case Settings.AHost:
                        AHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.A.ToString()].Host;
                        else
                            Config.Island[IslandsCode.A.ToString()].Host = model.SettingValue;
                        AHostBox.Text = model.SettingValue;
                        break;
                    case Settings.APictureHost:
                        APictureHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.A.ToString()].PictureHost;
                        else
                            Config.Island[IslandsCode.A.ToString()].PictureHost = model.SettingValue;
                        APictureHostBox.Text = model.SettingValue;
                        break;

                    case Settings.KHost:
                        KHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.Koukuko.ToString()].Host;
                        else
                            Config.Island[IslandsCode.Koukuko.ToString()].Host = model.SettingValue;
                        KHostBox.Text = model.SettingValue;
                        break;
                    case Settings.KPictureHost:
                        KPictureHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.Koukuko.ToString()].PictureHost;
                        else
                            Config.Island[IslandsCode.Koukuko.ToString()].PictureHost = model.SettingValue;
                        KPictureHostBox.Text = model.SettingValue;
                        break;

                    case Settings.BHost:
                        BHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.Beitai.ToString()].Host;
                        else
                            Config.Island[IslandsCode.Beitai.ToString()].Host = model.SettingValue;
                        BHostBox.Text = model.SettingValue;
                        break;
                    case Settings.BPictureHost:
                        APictureHostBox.Tag = model;
                        if (string.IsNullOrEmpty(model.SettingValue))
                            model.SettingValue = Config.Island[IslandsCode.Beitai.ToString()].PictureHost;
                        else
                            Config.Island[IslandsCode.Beitai.ToString()].PictureHost = model.SettingValue;
                        BPictureHostBox.Text = model.SettingValue;
                        break;
                }
            }
            if (string.IsNullOrEmpty(AHostBox.Text))
                AHostBox.Text = Config.Island[IslandsCode.A.ToString()].Host;
            if (string.IsNullOrEmpty(APictureHostBox.Text))
                APictureHostBox.Text = Config.Island[IslandsCode.A.ToString()].PictureHost;
            if (string.IsNullOrEmpty(BHostBox.Text))
                BHostBox.Text = Config.Island[IslandsCode.Beitai.ToString()].Host;
            if (string.IsNullOrEmpty(BPictureHostBox.Text))
                BPictureHostBox.Text = Config.Island[IslandsCode.Beitai.ToString()].PictureHost;
        }

        private async void BackgroundImagePathChange(string path)
        {
            MainPage.Global.BackgroundImagePath = path;
            BackgroundImagePathChanged?.Invoke(this, path);
            var model = BackgroundImageSettingModel;
            if (model == null)
            {
                model = new SettingModel()
                {
                    SettingName = Settings.BackgroundImagePath
                };
                BackgroundImageSettingModel = model;
            }
            var SettingValue = MainPage.Global.BackgroundImagePath;

            if (model.SettingValue != SettingValue)
            {
                model.SettingValue = SettingValue;
                await SaveSettingAsync(model);
            }
        }
        //private void UpdateSetting()
        //{
        //    string SettingValue = string.Empty;
        //    SettingModel model = null;

        //    model = IsHideMenuSwitch.Tag as SettingModel;
        //    if (model == null || model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.IsHideMenu = IsHideMenu;
        //    }

        //    model = TitleFontSizeSlider.Tag as SettingModel;
        //    if (model == null || model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.TitleFontSize = TitleFontSize;
        //    }

        //    model = ContentFontSizeSlider.Tag as SettingModel;
        //    if (model == null || model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.ContentFontSize = ContentFontSize;
        //    }
        //}

        //private async void SaveSettingAsync()
        //{
        //    string SettingValue = string.Empty;
        //    SettingModel model = null;
        //    #region 保存夜间模式
        //    model = NightModeSwitch.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.NightMode
        //    };
        //    SettingValue = NightModelIsOn ? "on" : string.Empty;

        //    if (model.SettingValue != SettingValue)
        //    {
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            NightModeSwitch.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存标题字号
        //    model = TitleFontSizeSlider.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.TitleFontSize
        //    };
        //    SettingValue = TitleFontSize.ToString();

        //    if (model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.TitleFontSize = TitleFontSize;
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            TitleFontSizeSlider.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存内容字号
        //    model = ContentFontSizeSlider.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.ContentFontSize
        //    };
        //    SettingValue = ContentFontSize.ToString();

        //    if (model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.ContentFontSize = ContentFontSize;
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            ContentFontSizeSlider.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存遮罩透明度
        //    model = MaskOpacitySlider.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.MaskOpacity
        //    };
        //    SettingValue = MaskOpacitySliderValue.ToString();

        //    if (model.SettingValue != SettingValue)
        //    {
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            MaskOpacitySlider.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存是否每次询问
        //    model = IsAskEachTimeBox.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.IsAskEachTime
        //    };
        //    SettingValue = IsAskEachTime ? "on" : string.Empty;

        //    if (model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.IsAskEachTime = IsAskEachTime;
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            IsAskEachTimeBox.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 背景图片
        //    model = BackgroundImageSettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.BackgroundImagePath
        //    };
        //    SettingValue = MainPage.Global.BackgroundImagePath;

        //    if (model.SettingValue != SettingValue)
        //    {
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            BackgroundImageSettingModel = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存隐藏菜单
        //    model = IsHideMenuSwitch.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.IsHideMenu
        //    };
        //    SettingValue = IsHideMenu ? "on" : string.Empty;

        //    if (model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.IsHideMenu = IsHideMenu;
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            IsHideMenuSwitch.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion

        //    #region 保存无图模式
        //    model = NoImageSwitch.Tag as SettingModel;
        //    if (model == null) model = new SettingModel()
        //    {
        //        SettingName = Settings.NoImage
        //    };
        //    SettingValue = NoImage ? "on" : string.Empty;

        //    if (model.SettingValue != SettingValue)
        //    {
        //        MainPage.Global.NoImage = NoImage;
        //        model.SettingValue = SettingValue;
        //        if (model._id == 0)
        //        {
        //            await Database.InsertAsync(model);
        //            NoImageSwitch.Tag = model;
        //        }
        //        else await Database.UpdateAsync(model);
        //    }
        //    #endregion
        //}

        private async Task SaveSettingAsync(List<SettingModel> list)
        {
            foreach (var model in list)
            {
                await SaveSettingAsync(model);
            }
        }

        private async Task SaveSettingAsync(SettingModel model)
        {
            if (model._id == 0) await Database.InsertAsync(model);
            else await Database.UpdateAsync(model);
        }
        
        private async void RoamingDataAsync()
        {
            int roamingMarkCount = 0;
            int roamingMyReplyCount = 0;
            int localMarkCount = 0;
            int localMyReplyCount = 0;
            await Task.Run(() =>
            {
                var roamingMarkList = Database.RoamingGetMarkList();
                var roamingMyReplyList = Database.RoamingGetMyReplyList();
                var markList = Database.GetMarkList(IslandsCode.All);
                var replyList = Database.GetMyReplyList(IslandsCode.All);
                var saveToRoamingMarkList = markList.Where(x => !roamingMarkList.Exists(y => y.id == x.id && y.islandCode == x.islandCode)).ToList();
                var saveToRoamingMyReplyList = replyList.Where(x =>
                !roamingMyReplyList.Exists(y => y.sendDateTime == x.sendDateTime && y.sendContent == x.sendContent && y.islandCode == x.islandCode)).ToList();

                var saveToLocalMarkList = roamingMarkList.Where(x => !markList.Exists(y => y.id == x.id && y.islandCode == x.islandCode)).ToList();
                var saveToMyReplyList = roamingMyReplyList.Where(x =>
                !replyList.Exists(y => y.sendDateTime == x.sendDateTime && y.sendContent == x.sendContent && y.islandCode == x.islandCode)).ToList();

                saveToLocalMarkList.ForEach(x =>
                {
                    if (Database.Insert(x) > 0) localMarkCount++;
                });
                saveToMyReplyList.ForEach(x =>
                {
                    if (Database.Insert(x) > 0) localMyReplyCount++;
                });
                saveToRoamingMarkList.ForEach(x =>
                {
                    if (Database.RoamingInsert(x) > 0) roamingMarkCount++;
                });
                saveToRoamingMyReplyList.ForEach(x =>
                {
                    if (Database.RoamingInsert(x) > 0) roamingMyReplyCount++;
                });
            });

            Data.Message.ShowMessage(string.Format("同步到漫游：收藏{0}，回复{1}\r\n同步到本地：收藏{2}，回复{3}",
                roamingMarkCount, roamingMyReplyCount, localMarkCount, localMyReplyCount), "同步完毕");
            DataRoamingButton.Visibility = Visibility.Visible;
        }

        private void RefreshImage_Click(object sender, RoutedEventArgs e)
        {
            RefreshImage();
        }

        private async void RefreshImage()
        {
            LocalImageView.ItemsSource = await File.GetLocalImageList();
        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            DeleteImage();
        }

        private async void DeleteImage()
        {
            if (LocalImageView.SelectedItems.Count == 0) return;
            foreach (StorageFile x in LocalImageView.SelectedItems)
            {
                try
                {
                    await x.DeleteAsync();
                } catch
                {
                }
            }
            RefreshImage();
        }

        private async void WindowsStore_Click(object sender, RoutedEventArgs e)
        {
            string uri = "ms-windows-store://review/?ProductId=9nblggh4v7wz";
            await Windows.System.Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}