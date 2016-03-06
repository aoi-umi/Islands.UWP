using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class AThreadModel
    {
        public string id { get; set; }
        public string img { get; set; }
        public string ext { get; set; }
        public string now { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string sage { get; set; }
        public string admin { get; set; }
        public string remainReplys { get; set; }
        public string replyCount { get; set; }
        public AThreadModel() { }
    }
}
