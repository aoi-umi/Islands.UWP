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
            MainControl mainControlA = new MainControl() {
            Host= Config.A.Host,
            PictureHost = Config.A.PictureHost,
            GetThreadAPI= Config.A.GetThreadAPI,
            GetReplyAPI = Config.A.GetReplyAPI,
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
                PageSize = Config.B.PageSize,
                IslandCode = IslandsCode.Beitai
            };
            pivotItemB.Content = mainControlB;
        }
    }
}
