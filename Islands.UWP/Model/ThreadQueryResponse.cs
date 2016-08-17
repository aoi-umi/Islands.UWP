using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Islands.UWP.Model
{
    public class ABThreadQueryResponse
    {
        public List<ThreadModel> threads;
    }

    public class KThreadQueryResponse
    {
        public KThreadQueryResObj data;
    }

    public class KThreadQueryResObj
    {
        public List<ThreadModel> threads;
    }
}
