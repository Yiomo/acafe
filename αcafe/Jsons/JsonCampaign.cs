using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Jsons
{
    class JsonCampaign
    {
        public class CampaignInfoListSorted
        {
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

        public class RootObject
        {
            public string code { get; set; }
            public string user_id { get; set; }
            public string program_id { get; set; }
            public List<CampaignInfoListSorted> CampaignInfoListSorted { get; set; }
        }
    }
}
