using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using αcafe.Jsons;
using αcafe.Functions;

namespace αcafe.Info
{
    public class RawInfo
    {
        public static Task<List<RawInfo>> GetRawInfoAsync(string fileId)
        {
            return Task.Run(() =>
            {
                HttpClient request = new HttpClient();
                {
                    request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }
                HttpResponseMessage responseD = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileDetail&fileId=" + fileId).Result;
                HttpResponseMessage responseC = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/fileComment.do?methodName=getComments&fileId=" + fileId).Result;
                string d = responseD.Content.ReadAsStringAsync().Result.Replace("Exposure Time", "Exposure_Time");
                string c = responseC.Content.ReadAsStringAsync().Result;
                bool a = LoginStatus.LoginInfo().IsLogin;

                JsonRaw.RootObject rb1 = JsonConvert.DeserializeObject<JsonRaw.RootObject>(d);
                Jsons.JsonRawComs.RootObject rb2 = JsonConvert.DeserializeObject<Jsons.JsonRawComs.RootObject>(c);

                List<RawInfo> rawinfo = new List<RawInfo>();
                List<RawInfo .allcomments> acom = new List<allcomments>();

                List<string> campaignID = new List<string>();
                List<string> code = new List<string>();
                List<string> description = new List<string>();
                List<string> dislikeNo = new List<string>();
                List<string> dynamicInfo = new List<string>();
                List<string> filePath = new List<string>();
                List<string> fileStatus = new List<string>();
                List<string> fileType = new List<string>();
                List<string> fileUploadPath = new List<string>();
                List<string> likeNo = new List<string>();
                List<string> thumbImage = new List<string>();
                List<string> title = new List<string>();
                List<string> userID = new List<string>();
                List<string> uploadTimeStamp = new List<string>();
                List<string> viewCount = new List<string>();

                List<string> Model = new List<string>();
                List<string> Theme = new List<string>();
                List<string> Lens = new List<string>();
                List<string> Exposure_Time = new List<string>();
                List<string> FNumber = new List<string>();
                List<string> ISO = new List<string>();

                List<string> commentId = new List<string>();
                List<string> commentStatus = new List<string>();
                List<string> commentedDate = new List<string>();
                List<string> comments = new List<string>();
                List<string> userIconPath = new List<string>();
                List<string> userId = new List<string>();

                campaignID.Add(rb1.campaignID);
                code.Add(rb1.code);
                description.Add(rb1.description);
                dislikeNo.Add(rb1.dislikeNo);
                filePath.Add(rb1.filePath);
                fileStatus.Add(rb1.fileStatus);
                fileType.Add(rb1.fileType);
                fileUploadPath.Add(rb1.fileUploadPath);
                likeNo.Add(rb1.likeNo);
                thumbImage.Add(rb1.thumbImage);
                title.Add(rb1.title);
                userID.Add(rb1.userID);
                uploadTimeStamp.Add(rb1.uploadTimeStamp);
                viewCount.Add(rb1.viewCount);

                JsonImage.DynamicInfo di = JsonConvert.DeserializeObject<JsonImage.DynamicInfo>(rb1.dynamicInfo);
                Model.Add(di.Model);
                Theme.Add(di.Theme);
                Lens.Add(di.Lens);
                Exposure_Time.Add(di.Exposure_Time);
                FNumber.Add(di.FNumber);
                ISO.Add(di.ISO);

                foreach (Jsons.JsonRawComs.Comments cm in rb2.comments)
                {
                    commentId.Add(cm.commentId);
                    commentStatus.Add(cm.commentStatus);
                    commentedDate.Add(cm.commentedDate);
                    comments.Add(cm.comments);
                    userIconPath.Add(cm.userIconPath);
                    userId.Add(cm.userId);
                }

                for (int i = 0; i < commentId.Count; i++)
                {
                    RawInfo.allcomments AC = new allcomments();
                    AC.commentId = commentId[i];
                    AC.commentStatus = commentStatus[i];
                    AC.commentedDate = commentedDate[i];
                    AC.comments = comments[i];
                    AC.userIconPath = userIconPath[i];
                    AC.userId = userId[i];
                    acom.Add(AC);
                }

                for (int i = 0; i < campaignID.Count; i++)
                {
                    RawInfo RI = new RawInfo();
                    RI.campaignID = campaignID[i];
                    RI.code = code[i];
                    RI.description = description[i];
                    RI.dislikeNo = dislikeNo[i];
                    RI.filePath = filePath[i];
                    RI.fileStatus = fileStatus[i];
                    RI.fileType = fileType[i];
                    RI.fileUploadPath = fileUploadPath[i];
                    RI.likeNo = likeNo[i];
                    RI.thumbImage = thumbImage[i];
                    RI.title = title[i];
                    RI.userID = userID[i];
                    RI.uploadTimeStamp = uploadTimeStamp[i];
                    RI.viewCount = viewCount[i];
                    RI.Model = Model[i];
                    RI.Theme = Theme[i];
                    RI.Lens = Lens[i];
                    RI.Exposure_Time = Exposure_Time[i];
                    RI.FNumber = FNumber[i];
                    RI.ISO = ISO[i];
                    RI.allComs = acom;
                    RI.comCount = acom.Count.ToString();
                    RI.fileId = fileId;
                    RI.loginstatus = a;
                    rawinfo.Add(RI);
                }
                return rawinfo;
            });
        }

        internal static Task<List<RawInfo>> GetRawInfoAsync(object fileId)
        {
            throw new NotImplementedException();
        }

        public string fileId { get; set; }
        public string campaignID { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string dislikeNo { get; set; }
        public string dynamicInfo { get; set; }
        public string filePath { get; set; }
        public string fileStatus { get; set; }
        public string fileType { get; set; }
        public string fileUploadPath { get; set; }
        public string likeNo { get; set; }
        public string thumbImage { get; set; }
        public string title { get; set; }
        public string uploadTimeStamp { get; set; }
        public string userID { get; set; }
        public string viewCount { get; set; }
        public string comCount { get; set; }
        public bool loginstatus { get; set; }

        public string Model { get; set; }
        public string Theme { get; set; }
        public string Lens { get; set; }
        public string Exposure_Time { get; set; }
        public string FNumber { get; set; }
        public string ISO { get; set; }
        public List<allcomments> allComs { get; set; }

        public class allcomments
        {
            public string commentId { get; set; }
            public string commentStatus { get; set; }
            public string commentedDate { get; set; }
            public string comments { get; set; }
            public string userIconPath { get; set; }
            public string userId { get; set; }
            public string count { get; set; }
            public string fileId { get; set; }
        }
    }

}
