using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplyView : BaseItemView
    {
        public ReplyView()
        {
            InitializeComponent();
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
                    if (ViewModel == null)
                    {
                        ViewModel = new ItemViewModel(_Reply) { GlobalConfig = MainPage.Global };
                    }
                    else
                        ViewModel.BaseItem = _Reply;
                }
            }
        }
        public string PoId { get; set; }
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();
            if (ViewModel != null)
            {
                if (ViewModel.IsAdmin) ViewModel.ItemUid += Config.AdminString;
                else if (ViewModel.IsPo) ViewModel.ItemUid += Config.PoString;
            }
            //    if (ViewModel != null && ViewModel.UserColor != null)
            //        uid.Foreground = ViewModel.UserColor;
            //    else
            //    {
            //        var bindingModel = new BindingModel()
            //        {
            //            BindingElement = uid,
            //            Source = createDate,
            //            Path = "Foreground",
            //            Property = TextBlock.ForegroundProperty,
            //        };
            //        Helper.BindingHelper(bindingModel);
            //    }            
        }   

        protected override async void OnRefClick(string RefText)
        {
            base.OnRefClick(RefText);
            try
            {                
                var refItem = GetRefByList(RefText, false, false);
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
