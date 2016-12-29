using Islands.UWP.Model;
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
                return;
                var viewModel = new ItemViewModel() { GlobalConfig = MainPage.Global, BaseItem = Reply };
                viewModel.IsTextSelectionEnabled = IsTextSelectionEnabled;
                BaseInit(viewModel);
                DataContext = viewModel;
            }
            base.OnLoaded();
            if (IsAdmin) txtUserid.Foreground = Config.AdminColor;
            else if (IsPo) txtUserid.Foreground = Config.PoColor;
        }

        private DependencyObject GetParent(DependencyObject reference, Type targetType)
        {
            var parent = VisualTreeHelper.GetParent(reference);
            if (parent == null || parent.GetType() == targetType)
                return parent;
            return GetParent(parent, targetType);
        }

        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);

            string id = RefText.ToLower().Replace(">>", "").Replace("no.", "");
            if (string.IsNullOrEmpty(id)) return;
            //从list寻找
            var lv = GetParent(this, typeof(ReplysView)) as ReplysView;
            if (lv != null)
            {
                foreach (var lvi in lv.Items)
                {
                    var model = lvi as DataModel;
                    if (model != null)
                    {
                        var item = model.Data as BaseItemModel;
                        if (item != null && item.id == id)
                        {
                            if (item is ThreadModel)
                            {
                                ThreadView thread = new ThreadView()
                                {
                                    Thread = item as ThreadModel,
                                    Margin = new Thickness(0, 0, 5, 0),
                                    IsTextSelectionEnabled = true
                                };
                                await Data.Message.ShowRef(RefText, thread);
                                return;
                            }
                            else if (item is ReplyModel)
                            {
                                ReplyView reply = new ReplyView() { Reply = item as ReplyModel, Margin = new Thickness(0, 0, 5, 0) };
                                await Data.Message.ShowRef(RefText, reply);
                                return;
                            }
                        }
                    }                    
                }
            }

            //用api
            try
            {
                string req = String.Format(GetRefAPI, Host, id);
                string res = await Data.Http.GetData(req);
                ReplyModel rm = Data.Convert.RefStringToReplyModel(res, IslandCode);
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
