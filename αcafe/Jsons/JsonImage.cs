using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonImage
    {
        public class FileList
        {
            public string campaignID { get; set; }
            public string description { get; set; }
            public string dislikeNo { get; set; }
            public string fileID { get; set; }
            public string filePath { get; set; }
            public string fileStatus { get; set; }
            public string fileType { get; set; }
            public string likeNo { get; set; }
            public string thumbImage { get; set; }
            public string title { get; set; }
            public string uploadTimeStamp { get; set; }
            public string userID { get; set; }
            public string viewCount { get; set; }
            public string dynamicInfo { get; set; }
        }
        public class RootObject
        {
            public string code { get; set; }
            public List<FileList> fileList { get; set; }
        }
        public class DynamicInfo
        {
            public string Model { get; set; }
            public string Theme { get; set; }
            public string Lens { get; set; }
            public string Exposure_Time { get; set; }
            public string FNumber { get; set; }
            public string ISO { get; set; }
        }
    }
}
