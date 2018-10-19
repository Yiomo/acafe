using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using αcafe.Jsons;
using αcafe.Functions;
using System;

namespace αcafe.Info
{
    class CampaignInfo
    {
        public static Task<List<CampaignInfo>> GetCampaignInfo_PT(string requrl, string referer)
        {
            return Task.Run(()=> {

                string temp = requrl;
                HttpResponseMessage response;

                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                {
                    request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    request.DefaultRequestHeaders.Add("Referer", referer);
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }

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

                JsonCampaign.RootObject rb1 = JsonConvert.DeserializeObject<JsonCampaign.RootObject>(b);

                List<CampaignInfo> campaignInfo = new List<CampaignInfo>();
                List<string> user_id = new List<string>();
                List<string> campaign_desc = new List<string>();
                List<string> campaign_id = new List<string>();
                List<string> campaign_title = new List<string>();
                List<string> cover_image_path = new List<string>();
                List<string> icon_path = new List<string>();

                foreach (JsonCampaign.CampaignInfoListSorted cl in rb1.CampaignInfoListSorted)
                {
                    user_id.Add(rb1.user_id);
                    campaign_desc.Add(cl.campaign_desc);
                    campaign_id.Add(cl.campaign_id);
                    campaign_title.Add(cl.campaign_title);
                    cover_image_path.Add(cl.cover_image_path);
                    icon_path.Add(cl.icon_path);
                }
                for (int i = 0; i < campaign_id.Count; i++)
                {
                    CampaignInfo CI = new CampaignInfo
                    {
                        user_id = user_id[i],
                        campaign_desc = campaign_desc[i],
                        campaign_id = campaign_id[i],
                        campaign_title = campaign_title[i],
                        cover_image_path = cover_image_path[i],
                        icon_path = icon_path[i],
                    };
                    campaignInfo.Add(CI);
                }
                return campaignInfo;
            });
        }

        public static Task<List<CampaignInfo>> GetNowCampaignInfo()
        {
            return Task.Run(()=> 
            {
                HttpResponseMessage response;

                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                {
                    request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                    request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                }

                response = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/programInfo.do?methodName=getProgramDetails&programId=1").Result;
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

                JsonAllCampaigns.RootObject rb1 = JsonConvert.DeserializeObject<JsonAllCampaigns.RootObject>(b);

                List<CampaignInfo> campaignInfo = new List<CampaignInfo>();
                List<string> campaign_desc = new List<string>();
                List<string> campaign_id = new List<string>();
                List<string> campaign_title = new List<string>();
                List<string> cover_image_path = new List<string>();
                List<string> icon_path = new List<string>();
                List<string> start_date = new List<string>();
                List<string> end_date = new List<string>();

                foreach (JsonAllCampaigns.CampaignInfoList cl in rb1.campaignInfoList)
                {
                    if (DateTime.Compare(DateTime.Now, Convert.ToDateTime(cl.end_date)) < 0&&cl.campaign_desc.Contains("test")==false )
                    {
                        campaign_desc.Add(cl.campaign_desc);
                        campaign_id.Add(cl.campaign_id);
                        campaign_title.Add(cl.campaign_title);
                        cover_image_path.Add(cl.cover_image_path);
                        icon_path.Add(cl.icon_path);
                        end_date.Add(cl.end_date);
                    }
                }

                for (int i = 0; i < campaign_id.Count; i++)
                {
                    CampaignInfo CI = new CampaignInfo
                    {
                        campaign_desc = campaign_desc[i],
                        campaign_id = campaign_id[i],
                        campaign_title = campaign_title[i],
                        cover_image_path = cover_image_path[i],
                        icon_path = icon_path[i],
                        end_date = end_date[i],
                    };
                    campaignInfo.Add(CI);
                }
                return campaignInfo;
            });
        }

        public string user_id { get; set; }
        public string campaign_control { get; set; }
        public string campaign_desc { get; set; }
        public string campaign_id { get; set; }
        public string campaign_title { get; set; }
        public string count { get; set; }
        public string cover_image_path { get; set; }
        public string ecoupon { get; set; }
        public string ecoupon_flag { get; set; }
        public string end_date { get; set; }
        public string hide_flag { get; set; }
        public string icon_path { get; set; }
        public string ip_flag { get; set; }
        public string start_date { get; set; }
    }
}
