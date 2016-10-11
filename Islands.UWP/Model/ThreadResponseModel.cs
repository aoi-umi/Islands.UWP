using System.Collections.Generic;

namespace Islands.UWP.Model
{
    public class ThreadResponseModel:ThreadModel
    {
        public List<ReplyModel> replys { get; set; }
        public List<ReplyModel> recentReply { get; set; }
    }
}
