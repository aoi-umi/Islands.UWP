using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ThreadView : BaseItemView
    {
        public ThreadView()
        {
            InitializeComponent();
            NoImage = MainPage.Global.NoImage;
        }

        public ThreadModel Thread
        {
            get { return (ThreadModel)GetValue(ThreadProperty); }
            set { SetValue(ThreadProperty, value); }
        }
        
        public static readonly DependencyProperty ThreadProperty =
            DependencyProperty.Register(nameof(Thread), typeof(ThreadModel), typeof(ThreadView), new PropertyMetadata(null));


        //public Model.ThreadModel Thread { get; set; }

        protected override void OnApplyTemplate()
        {
            var viewModel = new ItemViewModel(MainPage.Global, Thread);
            viewModel.IsTextSelectionEnabled = IsTextSelectionEnabled;
            viewModel.ItemReplyCount = Thread.replyCount;
            if (IsTextSelectionEnabled) SetRefClick(viewModel.ItemContentView);
            BaseInit(viewModel);
            DataContext = viewModel;

            base.OnApplyTemplate();
            if (IsAdmin) txtUserid.Foreground = Config.AdminColor;
            else if (IsPo) txtUserid.Foreground = Config.PoColor;
        }

        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);
            string id = RefText.ToLower().Replace(">>", "").Replace("no.", "");
            if (string.IsNullOrEmpty(id)) return;

            //用api
            try
            {
                string req = String.Format(GetRefAPI, Host, id);
                string res = await Data.Http.GetData(req);
                Model.ReplyModel rm = Data.Convert.RefStringToReplyModel(res, IslandCode);
                rm.islandCode = IslandCode;
                ReplyView reply = new ReplyView() { Reply = rm, Margin = new Thickness(0, 0, 5, 0) };
                await Data.Message.ShowRef(RefText, reply);
            }
            catch (Exception ex)
            {
                Data.Message.ShowMessage(ex.Message);
                return;
            }
        }
    }

}
