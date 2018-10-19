using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using αcafe.Functions;
using αcafe.Jsons;

namespace αcafe.Info
{
    class FollowerInfo
    {
        public static Task<List<FollowerInfo>> GetMyFollowersAsync(string userid)
        {
            return Task.Run(() => {
                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                HttpResponseMessage MyFolResponse = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/program.do?methodName=getMyFollowersList&programId=1" + "&userId=" + userid).Result;
                string a = MyFolResponse.Content.ReadAsStringAsync().Result;
                JsonFollower.RootObject rb1 = JsonConvert.DeserializeObject<JsonFollower.RootObject>(a);
                List<FollowerInfo> FolInfo = new List<FollowerInfo >();
                List<string> followerDate = new List<string>();
                List<string> follower_id = new List<string>();
                List<string> userIconPath = new List<string>();
                List<string> programId = new List<string>();
                List<string> userId = new List<string>();

                foreach (JsonFollower.FollowersList fl in rb1.followersList)
                {
                    followerDate.Add(fl.followerDate);
                    follower_id.Add(fl.follower_id);
                    userIconPath.Add(fl.userIconPath);
                }

                for (int i = 0; i < follower_id.Count; i++)
                {
                    FollowerInfo FI = new FollowerInfo();
                    FI.followerDate = followerDate[i];
                    FI.follower_id = follower_id[i];
                    FI.userIconPath = userIconPath[i];
                    FolInfo.Add(FI);
                }
                return FolInfo;
            });
        }

        public static Task<List<FollowerInfo>> GetIamFollowersAsync(string userid)
        {
            return Task.Run(() => {
                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                HttpResponseMessage MyFolResponse = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/program.do?methodName=getIamFollowingList&programId=1" + "&userId=" + userid).Result;
                string a = MyFolResponse.Content.ReadAsStringAsync().Result;
                JsonFollower.RootObject rb1 = JsonConvert.DeserializeObject<JsonFollower.RootObject>(a);
                List<FollowerInfo> FolInfo = new List<FollowerInfo>();
                List<string> followingDate = new List<string>();
                List<string> following_id = new List<string>();
                List<string> userIconPath = new List<string>();
                List<string> programId = new List<string>();
                List<string> userId = new List<string>();

                foreach (JsonFollower.Followering fl in rb1.following)
                {
                    followingDate.Add(fl.followingDate);
                    following_id.Add(fl.following_id);
                    userIconPath.Add(fl.userIconPath);
                }

                for (int i = 0; i < following_id.Count; i++)
                {
                    FollowerInfo FI = new FollowerInfo();
                    FI.followerDate = followingDate[i];
                    FI.follower_id = following_id[i];
                    FI.userIconPath = userIconPath[i];
                    FolInfo.Add(FI);
                }
                return FolInfo;
            });
        }

        public string followerDate { get; set; }
        public string follower_id { get; set; }
        public string userIconPath { get; set; }
        public string programId { get; set; }
        public string userId { get; set; }
        public string following_id { get; set; }
        public string followingDate { get; set; }
    }
}
