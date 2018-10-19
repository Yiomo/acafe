using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonAllCampaigns
    {
        public class CampaignInfoList
        {
            public string campaign_control { get; set; }
            public string campaign_desc { get; set; }
            public string campaign_id { get; set; }
            public string campaign_title { get; set; }
            public string cover_image_path { get; set; }
            public string ecoupon { get; set; }
            public string ecoupon_flag { get; set; }
            public string end_date { get; set; }
            public string hide_flag { get; set; }
            public string icon_path { get; set; }
            public string ip_flag { get; set; }
            public string special_campaign { get; set; }
            public string start_date { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public string program_id { get; set; }
            public string program_name { get; set; }
            public string program_desc { get; set; }
            public string icon_image_path { get; set; }
            public string cover_image_path { get; set; }
            public string default_icon_image { get; set; }
            public List<CampaignInfoList> campaignInfoList { get; set; }
        }
    }
}
