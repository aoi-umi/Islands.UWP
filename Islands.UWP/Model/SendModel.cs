using SQLite.Net.Attributes;

namespace Islands.UWP.Model
{
    public class SendModel
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }

        public string Host { get; set; }
        public string PostApi { get; set; }
        public string CookieName { get; set; }
        public string CookieValue { get; set; }
        public string sendTitle { get; set; }
        public string sendEmail { get; set; }
        public string sendName { get; set; }
        public string sendContent { get; set; }
        public string sendImage { get; set; }
        public string sendId { get; set; }
        public bool isMain { get; set; }
        public IslandsCode islandCode { get; set; }
        public string ThreadId { get; set; }
        public string sendDateTime { get; set; }
        public SendModel() {}
    }
}
