// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Islands.UWP.Model;

namespace Islands.UWP
{
    public sealed partial class MyReplyView : BaseItemView
    {
        public MyReplyView()
        {
            this.InitializeComponent();
            IsLocalImage = true;
        }
        public SendModel MyReply { get; set; }
    }
}
