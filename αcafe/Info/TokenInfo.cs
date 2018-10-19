using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using αcafe.Functions;

namespace αcafe.Info
{
    class TokenInfo
    {
        public static Task<TokenInfo> GetTokenAsync()
        {
            return Task.Run(()=> {
                string uzrUrl = "http://www.sonystyle.com.cn/mysony/campaign/api/token.do?methodName=generateToken&tokenType=1";
                string uplodUrl = "http://www.sonystyle.com.cn/mysony/campaign/api/token.do?methodName=generateToken&tokenType=2";
                HttpResponseMessage response1;
                HttpResponseMessage response2;

                HttpClient req1 = new HttpClient(Sign.HttpClientHandler);
                {
                    req1.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    req1.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    req1.DefaultRequestHeaders.Add("Referer", "http://www.sonystyle.com.cn/mysony/acafe/upload.htm");
                    req1.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    req1.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }
                HttpClient req2 = new HttpClient(Sign.HttpClientHandler);
                {
                    req2.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    req2.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    req2.DefaultRequestHeaders.Add("Referer", "http://www.sonystyle.com.cn/mysony/acafe/upload.htm");
                    req2.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    req2.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }

                response1 = req1.GetAsync(uzrUrl).Result;
                response2 = req2.GetAsync(uplodUrl).Result;

                string uzr_temp = response1.Content.ReadAsStringAsync().Result;
                string upl_temp = response2.Content.ReadAsStringAsync().Result;

                string uzrToken = Getresult(uzr_temp);
                string uplToken = Getresult(upl_temp);

                TokenInfo tokenInfo = new TokenInfo()
                {
                    userToken = uzrToken,
                    upLoadToken = uplToken,
                };
                return tokenInfo;
            });
        }

        public static string Getresult(string temp)
        {
            string[] s1 = temp.Split(',');
            string s2 = s1[2];
            string[] s3 = s2.Split(':');
            string s4 = s3[1].Replace('"',' ');
            return s4;
        }

        public string userToken;
        public string upLoadToken;
    }
}
