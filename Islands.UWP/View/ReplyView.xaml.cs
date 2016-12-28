using Islands.UWP.ViewModel;
using System;
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

        public Model.ReplyModel Reply { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnLoaded()
        {
            if (DataContext == null)
            {
                var viewModel = new ItemViewModel() { GlobalConfig = MainPage.Global, BaseItem = Reply };
                viewModel.IsTextSelectionEnabled = IsTextSelectionEnabled;
                BaseInit(viewModel);
                DataContext = viewModel;
            }
            base.OnLoaded();
            if (IsAdmin) txtUserid.Foreground = Config.AdminColor;
            else if (IsPo) txtUserid.Foreground = Config.PoColor;
        }

        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);

            string id = RefText.ToLower().Replace(">>", "").Replace("no.", "");
            if (string.IsNullOrEmpty(id)) return;
            //从list寻找
            //var p = VisualTreeHelper.GetParent(this);
            var lv = this.Parent as ReplysView;
            if (lv != null)
            {
                foreach (var lvi in lv.Items)
                {
                    ThreadView tv = lvi as ThreadView;
                    if (tv != null && tv.Thread != null && tv.Thread.id == id)
                    {
                        tv.Thread.islandCode = IslandCode;
                        ThreadView thread = new ThreadView()
                        {
                            Thread = tv.Thread,
                            Margin = new Thickness(0, 0, 5, 0),
                            Background = null,
                            IsTextSelectionEnabled = true
                        };
                        await Data.Message.ShowRef(RefText, thread);
                        return;
                    }
                    ReplyView rv = lvi as ReplyView;
                    if (rv != null && rv.Reply != null && rv.Reply.id == id)
                    {
                        rv.Reply.islandCode = IslandCode;
                        ReplyView reply = new ReplyView() { Reply = rv.Reply, Margin = new Thickness(0, 0, 5, 0) };
                        await Data.Message.ShowRef(RefText, reply);
                        return;
                    }
                }
            }

            //用api
            try
            {
                string req = String.Format(GetRefAPI, Host, id);
                string res = await Data.Http.GetData(req);
                Model.ReplyModel rm = Data.Convert.RefStringToReplyModel(res, IslandCode);
                rm.islandCode = IslandCode;
                ReplyView reply = new ReplyView() {Reply = rm, Margin = new Thickness(0, 0, 5, 0) };
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
