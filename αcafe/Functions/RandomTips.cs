using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace αcafe.Functions
{
    class RandomTips
    {
        public static string GetTips()
        {
            List<string> Tips = new List<string>();
            Random rd = new Random();

            Tips.Add("最佳的变焦镜便是你自己的脚");
            Tips.Add("多留意光线");
            Tips.Add("「如果你的照片拍的不够好，那是因为你靠的不够近。」");
            Tips.Add("不要想太多，拍摄吧！");
            Tips.Add("不要停止练习");
            Tips.Add("减少负重，增加创意");
            Tips.Add("大光圈小景深");
            Tips.Add("小光圈大景深");

            int a = rd.Next(0,Tips.Count-1);
            return Tips[a];
        }
    }
}
