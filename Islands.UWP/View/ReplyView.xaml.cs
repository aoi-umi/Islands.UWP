using System;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplyView : BaseItemView
    {
        public ReplyView(Model.ReplyModel reply, IslandsCode islandCode)
        {
            InitializeComponent();
            DataContext = MainPage.Global;
            this.reply = reply;
            IslandCode = islandCode;
            NoImage = MainPage.Global.NoImage;
            IsTextSelectionEnabled = true;
            BaseInit(reply as Model.BaseItemModel);
        }

        public Model.ReplyModel reply { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (IsAdmin) txtUserid.Foreground = Config.AdminColor;
            else if (IsPo) txtUserid.Foreground = Config.PoColor;
        }

        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);

            string id = RefText.ToLower().Replace(">>", "").Replace("no.", "");
            if (string.IsNullOrEmpty(id)) return;
            //从list寻找
            var lv = this.Parent as ReplysView;
            if (lv != null)
            {
                foreach (var lvi in lv.Items)
                {
                    ThreadView tv = lvi as ThreadView;
                    if (tv != null && tv.thread != null && tv.thread.id == id)
                    {
                        ThreadView thread = new ThreadView(tv.thread, IslandCode) { Margin = new Thickness(0, 0, 5, 0), Background = null, IsTextSelectionEnabled = true };
                        await Data.Message.ShowRef(RefText, thread);
                        return;
                    }
                    ReplyView rv = lvi as ReplyView;
                    if (rv != null && rv.reply != null && rv.reply.id == id)
                    {
                        ReplyView reply = new ReplyView(rv.reply, IslandCode) { Margin = new Thickness(0, 0, 5, 0) };
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
                ReplyView reply = new ReplyView(rm, IslandCode) { Margin = new Thickness(0, 0, 5, 0) };
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
