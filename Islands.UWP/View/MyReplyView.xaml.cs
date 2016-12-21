// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Islands.UWP
{
    public sealed partial class MyReplyView : BaseItemView
    {

        public MyReplyView(Model.SendModel myReply, IslandsCode islandCode)
        {
            this.InitializeComponent();
            this.myReply = myReply;
            this.islandCode = islandCode;            
            ItemTitle = string.IsNullOrEmpty(myReply.sendTitle) ? "" : "标题:" + myReply.sendTitle;
            ItemEmail = string.IsNullOrEmpty(myReply.sendEmail) ? "" : "email:" + myReply.sendEmail;
            ItemName = string.IsNullOrEmpty(myReply.sendName) ? "" : "名字:" + myReply.sendName;
            ItemNo = myReply.ThreadId;
            ItemCreateDate = myReply.sendDateTime;
            ItemMsg = (myReply.isMain ? "新串：" : "回复：") + myReply.sendId; 
            ItemThumb = ItemImage = myReply.sendImage;
            ItemContent = myReply.sendContent;
            IsLocalImage = true;
        }

        public Model.SendModel myReply {get; set; }

        private IslandsCode islandCode { get; set; }
    }
}
