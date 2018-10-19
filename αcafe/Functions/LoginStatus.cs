using System.Net.Http;

namespace αcafe.Functions
{
    class LoginStatus
    {
        public static Info LoginInfo()
        {
            HttpClient request = new HttpClient(Sign.HttpClientHandler);
            HttpResponseMessage response = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/user.do?methodName=getUserDetails&keys=userId").Result;
            string a = response.Content.ReadAsStringAsync().Result;
            bool b = false;
            string c = null;

            if (a.Contains("User not logged in"))
            {
                b = false;
                c = null;
            }
            else
            {
                b = true;
                string[] temp;
                temp = (a.Split(':'))[1].Split(',');
                c = temp[0].Replace('"', ' ');
            }

            Info info = new Info
            {
                IsLogin = b,
                userid = c,
            };
            return info;
        }

        public class Info
        {
            public bool IsLogin { get; set; }
            public string userid { get; set; }
        }
    }
}
