using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islands.UWP.Model
{
    public class ContentModel
    {
        public string ThreadID { get; set; }
        public string date { get; set; }
        public string uid { get; set; }
        public string ReplyCount { get; set; }
        public string ReplyTitle { get; set; }
        public string Information { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Thumb { get; set; }
        public ContentModel() { }
    }
}
