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
    }

    public enum DataTypes
    {
        Thread = 0,
        Reply = 1,
        PageInfo = 2,
        BottomInfo = 3
    }
}
