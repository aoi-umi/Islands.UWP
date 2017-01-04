namespace Islands.UWP.ViewModel
{
    public class DataModel
    {
        public DataTypes DataType { get; set; }
        public object Data { get; set; }
        public object Parameter { get; set; }
    }

    public enum DataTypes
    {
        None,
        Thread,
        Mark,
        Reply,
        MyReply,
        PageInfo,
        BottomInfo
    }

    public class ItemParameter
    {
        public bool IsTextSelectionEnabled { get; set; }
        public bool IsPo { get; set; }
        public bool IsRef { get; set; }
        public object ParentList { get; set; }
    }
}
