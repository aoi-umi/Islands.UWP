using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Thread,
        Reply,
        MyReply,
        PageInfo,
        BottomInfo
    }

    public class ItemParameter
    {
        public bool IsTextSelectionEnabled { get; set; }
        public bool IsPo { get; set; }
    }
}
