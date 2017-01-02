﻿using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using UmiAoi.UWP;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadView : BaseItemView
    {
        public ThreadView() : base()
        {
            InitializeComponent();
            NoImage = MainPage.Global.NoImage;
        }

        private ThreadModel _Thread { get; set; }
        public ThreadModel Thread
        {
            get { return _Thread; }
            set
            {
                if (_Thread != value)
                {
                    _Thread = value;
                    if (ViewModel == null)
                    {
                        ViewModel = new ItemViewModel(_Thread) { GlobalConfig = MainPage.Global };
                    }
                    else
                        ViewModel.BaseItem = _Thread;
                }
            }
        }

        public bool IsTextSelectionEnabled { set { if (ViewModel != null) ViewModel.IsTextSelectionEnabled = value; } }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();
            if (ViewModel != null && ViewModel.UserColor != null)
                uid.Foreground = ViewModel.UserColor;
            else
            {
                var bindingModel = new BindingModel()
                {
                    BindingElement = uid,
                    Source = createDate,
                    Path = "Foreground",
                    Property = TextBlock.ForegroundProperty,
                };
                Helper.BindingHelper(bindingModel);
            }
        }
        
        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);

            //用api
            try
            {
                var refItem = GetRefByApi(RefText);
                await Data.Message.ShowRef(RefText, refItem);
            }
            catch (Exception ex)
            {
                Data.Message.ShowMessage(ex.Message);
                return;
            }
        }
    }

}
