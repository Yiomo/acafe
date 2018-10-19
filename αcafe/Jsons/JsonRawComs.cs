using System.Collections.Generic;

namespace αcafe.Jsons
{
    class JsonRawComs
    {
        public class Comments
        {
            public string commentId { get; set; }
            public string commentStatus { get; set; }
            public string commentedDate { get; set; }
            public string comments { get; set; }
            public string userIconPath { get; set; }
            public string userId { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public List<Comments> comments { get; set; }
            public string count { get; set; }
            public string fileId { get; set; }
        }
    }
}
