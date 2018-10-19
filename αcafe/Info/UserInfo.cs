using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using αcafe.Jsons;
using αcafe.Functions;

namespace αcafe.Info
{
    class UserInfo
    {
        public static Task<List<UserInfo>> GetUserInfo_PT(string requrl, string referer)
        {
            return Task.Run(() => { 
                HttpResponseMessage response;
                HttpClient request = new HttpClient();
                {
                    request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    request.DefaultRequestHeaders.Add("Referer", referer);
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }
                List<UserInfo> userInfo = new List<UserInfo>();
                response = request.GetAsync(requrl).Result;
                string jsonstr = response.Content.ReadAsStringAsync().Result;

                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(jsonstr);
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);

                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                string b = textWriter.ToString();
                JsonUserInfo.RootObject rb1 = JsonConvert.DeserializeObject<JsonUserInfo.RootObject>(b);

                if (requrl.Contains("Model")==true)
                {
                    List<string> Model = new List<string>();
                    List<string> Count = new List<string>();
                    foreach (JsonUserInfo.GroupByCount gb in rb1.GroupByCount)
                    {
                        Model.Add(gb.Model);
                        Count.Add(gb.Count);
                    }
                    for (int i = 0; i < Model.Count; i++)
                    {
                        UserInfo JU = new UserInfo
                        {
                            Model = Model[i],
                            Count = Count[i],
                        };
                        userInfo.Add(JU);
                    }
                    return userInfo;
                } 
                else if (requrl.Contains("Lens")==true )
                {
                    List<string> Lens = new List<string>();
                    List<string> Count = new List<string>();
                    foreach (JsonUserInfo.GroupByCount gb in rb1.GroupByCount)
                    {
                        Lens.Add(gb.Model);
                        Count.Add(gb.Count);
                    }
                    for (int i = 0; i < Lens.Count; i++)
                    {
                        UserInfo JU = new UserInfo
                        {
                            Lens = Lens[i],
                            Count = Count[i],
                        };
                        userInfo.Add(JU);
                    }
                    return userInfo;
                } 
                else if (requrl.Contains("Theme")==true)
                {
                    List<string> Theme = new List<string>();
                    List<string> Count = new List<string>();
                    foreach (JsonUserInfo.GroupByCount gb in rb1.GroupByCount)
                    {
                        Theme.Add(gb.Theme);
                        Count.Add(gb.Count);
                    }
                    for (int i = 0; i < Theme.Count; i++)
                    {
                        UserInfo JU = new UserInfo
                        {
                            Theme = Theme[i],
                            Count = Count[i],
                        };
                        userInfo.Add(JU);
                    }
                    return userInfo;
                }
                return userInfo;
            });
        }


        public string Model { get; set; }
        public string Lens { get; set; }
        public string Theme { get; set; }
        public string Count { get; set; }
    }
}
