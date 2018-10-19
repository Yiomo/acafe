using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Functions
{
    class GetTips
    {
        public static string GetRdTips()
        {
            List<string> AllTips = new List<string>();
            AllTips.Add("最佳的变焦镜便是你自己的脚");
            AllTips.Add("多留意光线");
            AllTips.Add("靠近，然后再靠近一点");
            AllTips.Add("不要想太多，拍摄吧！");
            AllTips.Add("不要停止练习");
            AllTips.Add("减少你的负重，增加你的创意");
            AllTips.Add("多拍摄，不要光看理论");
            AllTips.Add("放慢一点");
            AllTips.Add("街拍宜低调，高感黑白片，旁轴大光圈");
            AllTips.Add("广角重主题，长焦压缩景");
            AllTips.Add("小光圈景深，全开糊背景");
            AllTips.Add("拍花侧逆光，慢门显动感");
            AllTips.Add("见山寻侧光，见水拍倒影");
            AllTips.Add("对焦对主题，水平要抓平");
            AllTips.Add("长曝避车灯，岩石要湿润");
            AllTips.Add("有云天要多，无云地为主");
            AllTips.Add("前景位关键，三分九宫格");
            AllTips.Add("偏光去反光，渐变平反差");
            AllTips.Add("拍小孩全靠手急，拍女孩全靠磨皮。");
            AllTips.Add("变焦基本靠走，对焦基本靠扭。");

            Random rd = new Random();
            int a=rd.Next(1,AllTips.Count()+1);
            return AllTips[a];
        }
    }
}
