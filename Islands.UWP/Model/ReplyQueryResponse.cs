using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class KReplyQueryResponse
    {
        public bool success;
        public string message;
        public ThreadModel threads;
        public List<ReplyModel> replys;
    }

    public class ABReplyQueryResponse : ThreadModel
    {
        public List<ReplyModel> replys;
    }
}
