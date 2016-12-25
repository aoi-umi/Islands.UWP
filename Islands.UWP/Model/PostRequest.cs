namespace Islands.UWP.Model
{
    public class PostRequest
    {
        public string Type { get; set; }
        public string API { get; set; }
        public string Host { get; set; }
        public string ID { get; set; }
        public int Page { get; set; } = 1;
        public PostRequest()
        {
        }
    }
}
