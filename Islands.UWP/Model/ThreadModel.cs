using SQLite.Net.Attributes;

namespace Islands.UWP.Model
{
    public class ThreadModel
    {
        public IslandsCode islandCode { get; set; }

        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
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

        public string uid { get; set; }
        public string image { get; set; }
        public string thumb { get; set; }
        public string forum {get;set;}
        public string updatedAt { get; set; }
        public string createdAt { get; set; }

        public ThreadModel() {}
    }
}
