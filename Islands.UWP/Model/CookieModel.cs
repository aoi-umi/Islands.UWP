using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islands.UWP.Model
{
    public class CookieModel
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
        public IslandsCode islandCode { get; set; }

        public string CookieName { get; set; }
        public string CookieValue { get; set; }
        public CookieModel() { }
    }
}
