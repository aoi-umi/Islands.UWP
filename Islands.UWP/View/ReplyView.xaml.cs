using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplyView : BaseItemView
    {
        public ReplyView()
        {
            InitializeComponent();
            NoImage = MainPage.Global.NoImage;
            IsTextSelectionEnabled = true;            
        }

        private ReplyModel _Reply { get; set; }
        public ReplyModel Reply
        {
            get { return _Reply; }
            set
            {
                if (value != _Reply)
                {
                    _Reply = value;
                    var viewModel = DataContext as ItemViewModel;
                    if (viewModel == null)
                    {
                        viewModel = new ItemViewModel() { GlobalConfig = MainPage.Global };
                        DataContext = viewModel;
                    }
                    viewModel.BaseItem = _Reply;
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
            try
            {                
                var refItem = GetRefByList(RefText);
                if (refItem == null) refItem = await GetRefByApi(RefText);
                await Data.Message.ShowRef(RefText, refItem);
            }
            catch (Exception ex)
            {
                Data.Message.ShowMessage(ex.Message);                
            }            
        }
    }
}
