using System.Collections.Generic;

namespace Islands.UWP.Model
{
    public class KReplyQueryResponse
    {
        public bool success;
        public string message;
        public ThreadModel threads;
        public List<ReplyModel> replys;
        public ReplyModel data;
    }

    public class ABReplyQueryResponse : ThreadModel
    {
        public List<ReplyModel> replys;
    }
}
