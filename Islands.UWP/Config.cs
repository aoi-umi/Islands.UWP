using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP
{
    public enum IslandsCode
    {
        A = 0,
        Koukuko = 1,
        Beitai = 2
    };
    public static class Config
    {
        public static class A
        {
            public static string Host = "http://h.nimingban.com";
            public static string PictureHost = "http://h-nimingban-com.n1.yun.tf:8999/Public/Upload/";
            public static string GetThreadAPI = "{0}/Api/showf/id/{1}/page/{2}";
            public static string GetReplyAPI = "{0}/Api/thread/id/{1}/page/{2}";
            public static int PageSize = 19;
        }
    }
}
