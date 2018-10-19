using System.Collections.Generic;
using System.Net.Http;
using System.Xml;

namespace αcafe.Functions
{
    class HttpXml
    {
        public static List<string> Responsetext(string requesturl, string category, string series, string type)
        {
            string temp = requesturl;
            HttpClient request = new HttpClient();
            {
                request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
            }
            HttpResponseMessage response = request.GetAsync(temp).Result;
            string jsonstr = response.Content.ReadAsStringAsync().Result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(jsonstr);

            XmlElement root = null;
            root = doc.DocumentElement;

            XmlNodeList nodeList = null;

            string fliterexp;
            if (type == "lens")
            {
                fliterexp = "//category[@name='" + category + "']/series[@name='" + series + "']/model";
            }
            else
            {
                fliterexp = "//category[@name='" + category + "']/model";
            }

            nodeList = root.SelectNodes(fliterexp);

            List<string> result = new List<string>();
            foreach (XmlNode node in nodeList)
            {
                result.Add(node.InnerText);
            }
            return result;
        }

    }
}
