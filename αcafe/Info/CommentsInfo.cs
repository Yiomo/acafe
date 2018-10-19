using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using αcafe.Functions;
using αcafe.Jsons;

namespace αcafe.Info
{
    class CommentsInfo
    {
        public static Task<List<CommentsInfo>> GetComInAsync()
        {
            return Task.Run(() => {
                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                HttpResponseMessage CmInResponse = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/fileComment.do?methodName=getCommentsOnMyfiles&programId=1").Result;
                string a = CmInResponse.Content.ReadAsStringAsync().Result;
                JsonComments.RootObject rb1 = JsonConvert.DeserializeObject<JsonComments.RootObject>(a);
                List<CommentsInfo> ComInInfo = new List<CommentsInfo>();
                List<string> commentId = new List<string>();
                List<string> commentStatus = new List<string>();
                List<string> commentedDate = new List<string>();
                List<string> comments = new List<string>();
                List<string> fileIconPath = new List<string>();
                List<string> fileId = new List<string>();
                List<string> fileTitle = new List<string>();
                List<string> userId = new List<string>();
                List<string> count = new List<string>();

                foreach (JsonComments.Comments cm in rb1.comments)
                {
                    commentId.Add(cm.commentId);
                    commentStatus.Add(cm.commentStatus);
                    commentedDate.Add(cm.commentedDate);
                    comments.Add(cm.comments);
                    fileIconPath.Add(cm.fileIconPath);
                    fileId.Add(cm.fileId);
                    fileTitle.Add(cm.fileTitle);
                    userId.Add(cm.userId);
                }

                for (int i = 0; i < comments.Count; i++)
                {
                    CommentsInfo CI = new CommentsInfo();
                    CI.commentId = commentId[i];
                    CI.commentStatus = commentStatus[i];
                    CI.commentedDate = commentedDate[i];
                    CI.comments = comments[i];
                    CI.fileIconPath = fileIconPath[i];
                    CI.fileId = fileId[i];
                    CI.fileTitle = fileTitle[i];
                    CI.userId = userId[i];
                    ComInInfo.Add(CI);
                }
                return ComInInfo;
            });
        }

        public static Task<List<CommentsInfo>> GetComOutAsync()
        {
            return Task.Run(() => {
                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                HttpResponseMessage CmInResponse = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/fileComment.do?methodName=getMyComments&programId=1").Result;
                string a = CmInResponse.Content.ReadAsStringAsync().Result;
                JsonComments.RootObject rb1 = JsonConvert.DeserializeObject<JsonComments.RootObject>(a);
                List<CommentsInfo> ComInInfo = new List<CommentsInfo>();
                List<string> commentId = new List<string>();
                List<string> commentStatus = new List<string>();
                List<string> commentedDate = new List<string>();
                List<string> comments = new List<string>();
                List<string> fileIconPath = new List<string>();
                List<string> fileId = new List<string>();
                List<string> fileTitle = new List<string>();
                List<string> userId = new List<string>();
                List<string> count = new List<string>();

                foreach (JsonComments.Comments cm in rb1.comments)
                {
                    commentId.Add(cm.commentId);
                    commentStatus.Add(cm.commentStatus);
                    commentedDate.Add(cm.commentedDate);
                    comments.Add(cm.comments);
                    fileIconPath.Add(cm.fileIconPath);
                    fileId.Add(cm.fileId);
                    fileTitle.Add(cm.fileTitle);
                    userId.Add(cm.userId);
                }

                for (int i = 0; i < comments.Count; i++)
                {
                    CommentsInfo CI = new CommentsInfo();
                    CI.commentId = commentId[i];
                    CI.commentStatus = commentStatus[i];
                    CI.commentedDate = commentedDate[i];
                    CI.comments = comments[i];
                    CI.fileIconPath = fileIconPath[i];
                    CI.fileId = fileId[i];
                    CI.fileTitle = fileTitle[i];
                    CI.userId = userId[i];
                    ComInInfo.Add(CI);
                }
                return ComInInfo;
            });

        }


        public string commentId { get; set; }
        public string commentStatus { get; set; }
        public string commentedDate { get; set; }
        public string comments { get; set; }
        public string fileIconPath { get; set; }
        public string fileId { get; set; }
        public string fileTitle { get; set; }
        public string userId { get; set; }
        public string count { get; set; }

    }
}
