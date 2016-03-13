using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class ResponseModel
    {
        public string body { get; set; }
        public IEnumerable<string> cookies { get; set; }
        public ResponseModel() { }
    }
}
