using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islands.UWP.Model
{
    public class BaseModel
    {
        public string Host { get; set; }
        public string PictureHost { get; set; }
        public string GetThreadAPI { get; set; }
        public string GetReplyAPI { get; set; }
        public string GetRefAPI { get; set; }
        public string PostThreadAPI { get; set; }
        public string PostReplyAPI { get; set; }
        public int PageSize { get; set; }
        public string CookieKey { get; set; }
        public BaseModel() { }
    }
}
