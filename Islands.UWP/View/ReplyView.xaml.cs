using Islands.UWP.Model;
using Islands.UWP.ViewModel;
using System;
using UmiAoi.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class ReplyView : BaseItemView
    {
        public ReplyView()
        {
            InitializeComponent();
            NoImage = MainPage.Global.NoImage;
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
            if (ViewModel != null && ViewModel.ItemContentView != null &&
                ViewModel.RefIdList != null && ViewModel.RefIdList.Count > 0)
            {
                var list = ViewModel.RefIdList;
                var view = ViewModel.ItemContentView;
                var p = new Paragraph();
                list.ForEach(x =>
                {
                    var model = GetRefByList(x, true);
                    if (model != null)
                    {
                        //var b = new BindingModel()
                        //{
                        //    Source = this,
                        //    Path = "ActualWidth",
                        //    BindingElement = model as FrameworkElement,
                        //    Property = FrameworkElement.WidthProperty,
                        //};
                        //Helper.BindingHelper(b);
                        var i = new InlineUIContainer();
                        var content = new ContentControl();
                        var m = new DataModel()
                        {
                            DataType = DataTypes.Thread,
                            Data = model,
                        };
                        content.DataContext = m;
                        i.Child = content;
                        if (model is ThreadModel)
                        {
                            m.Parameter = new ItemParameter() { IsRef = true, IsTextSelectionEnabled = true, IsPo = true };
                            content.ContentTemplate = ItemDataTemplateSelector.GetTemplate(DataTypes.Thread);
                        }
                        else if (model is ReplyModel)
                        {
                            m.DataType = DataTypes.Reply;
                            m.Parameter = new ItemParameter() { IsRef = true, IsTextSelectionEnabled = true };
                            content.ContentTemplate = ItemDataTemplateSelector.GetTemplate(DataTypes.Reply);
                        }
                        p.Inlines.Add(i);
                    }
                });
                if (p.Inlines.Count > 0)
                    view.Blocks.Add(p);
            }
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
