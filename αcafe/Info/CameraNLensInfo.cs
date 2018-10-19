using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using αcafe.Functions;

namespace αcafe.Info
{
    class CameraNLensInfo
    {
        public static List<string> GetCNLList(string type)
        {
            string cameraUrl = "http://www.sonystyle.com.cn/mysony/acafe/xml/camera.xml";
            string lensUrl = "http://www.sonystyle.com.cn/mysony/acafe/xml/lens.xml";

            HttpClient request = new HttpClient();
            {
                request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
            }

            if (type == "cameras")
            {
                HttpResponseMessage responseC = request.GetAsync(cameraUrl).Result;
                string jsonstrC = responseC.Content.ReadAsStringAsync().Result;
                XmlDocument docC = new XmlDocument();
                docC.LoadXml(jsonstrC);
                XmlElement rootC = null;
                rootC = docC.DocumentElement;
                XmlNodeList nodeListC = null;
                string fliterexpC;
                fliterexpC = "//category/model";
                nodeListC = rootC.SelectNodes(fliterexpC);
                List<string> resultC = new List<string>();
                foreach (XmlNode node in nodeListC)
                {
                    resultC.Add(node.InnerText);
                }
                return resultC;
            }
            else
            {
                HttpResponseMessage responseL = request.GetAsync(lensUrl).Result;
                string jsonstrL = responseL.Content.ReadAsStringAsync().Result;
                XmlDocument docL = new XmlDocument();
                docL.LoadXml(jsonstrL);
                XmlElement rootL = null;
                rootL = docL.DocumentElement;
                XmlNodeList nodeListL = null;
                string fliterexpL;
                fliterexpL = "//category/series/model";
                nodeListL = rootL.SelectNodes(fliterexpL);
                List<string> resultL = new List<string>();
                foreach (XmlNode node in nodeListL)
                {
                    resultL.Add(node.InnerText);
                }
                return resultL;
            }
        }
    }
}
