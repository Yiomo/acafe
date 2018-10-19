using System.Threading.Tasks;
using System.Net.Http;

namespace αcafe.Functions
{
    class Sign
    {
        public static HttpClientHandler HttpClientHandler = new HttpClientHandler() { UseCookies = true, };

        public static Task<HttpResponseMessage> SignIn(string Req)
        {
            return Task.Run(() => {
                HttpClient request = new HttpClient(HttpClientHandler);
                HttpResponseMessage response;
                {
                    request.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                    request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                }
                response = request.PostAsync(Req, null).Result;
                return response;
            });
        }

        public static Task<HttpResponseMessage> VerCode(string Req)
        {
            return Task.Run(() =>
            {
                HttpClient request = new HttpClient(HttpClientHandler);
                HttpResponseMessage response;
                {
                    request.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                    request.DefaultRequestHeaders.Add("Accept", "*/*");
                    request.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                }
                try
                {
                    response = request.GetAsync(Req).Result;
                }
                catch
                {
                    response = null;
                }
                return response;
            });
        }
    }
}
