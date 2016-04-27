using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class PostRequest
    {
        public string Type { get; set; }
        public string API { get; set; }
        public string Host { get; set; }
        public string ID { get; set; }
        public int Page { get; set; }
        public PostRequest()
        {
            Page = 1;
        }
    }
}
