using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using Windows.UI.Xaml;

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
                    var viewModel = DataContext as ItemViewModel;
                    if (viewModel == null)
                    {
                        viewModel = new ItemViewModel() { GlobalConfig = MainPage.Global };
                        viewModel.ItemReplyCount = _Thread.replyCount;
                        DataContext = viewModel;
                    }
                    viewModel.BaseItem = _Thread;
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnLoaded()
        {
            if (DataContext == null)
            {
                return;               
            }
            base.OnLoaded();
            if (IsAdmin) txtUserid.Foreground = Config.AdminColor;
            else if (IsPo) txtUserid.Foreground = Config.PoColor;
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
