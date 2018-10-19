using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonUserInfo
    {
        public class GroupByCount
        {
            public string Model { get; set; }
            public string Lens { get; set; }
            public string Theme { get; set; }
            public string Count { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public List<GroupByCount> GroupByCount { get; set; }
        }
    }
}
