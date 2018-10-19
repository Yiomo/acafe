using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonComments
    {
        public class Comments
        {
            public string commentId { get; set; }
            public string commentStatus { get; set; }
            public string commentedDate { get; set; }
            public string comments { get; set; }
            public string fileIconPath { get; set; }
            public string fileId { get; set; }
            public string fileTitle { get; set; }
            public string userId { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public List<Comments> comments { get; set; }
            public string count { get; set; }
            public string userId { get; set; }
        }

    }
}
