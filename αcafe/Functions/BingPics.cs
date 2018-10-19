using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using System.Xml;

namespace αcafe.Functions
{
    class BingPics
    {
        public static Task< bool> GetBingPics()
        {
            return Task.Run(async () =>
             {
                 string temp = "http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
                 string downtemp = "http://cn.bing.com";
                 string baseurlexp = "//images/image/url";
                 string copyrightexp = "//images/image/copyright";
                 string dateexp = "//images/image/enddate";

                 HttpClient request = new HttpClient();
                 {
                     request.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                     request.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
                     request.DefaultRequestHeaders.Add("Accept-Encoding", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                     request.DefaultRequestHeaders.Add("Accept-Language", "gzip, deflate");
                 }

                 try
                 {
                     HttpResponseMessage response = request.GetAsync(temp).Result;
                     string jsonstr = response.Content.ReadAsStringAsync().Result;
                     XmlDocument doc = new XmlDocument();
                     doc.LoadXml(jsonstr);

                     XmlElement root = null;
                     root = doc.DocumentElement;

                     XmlNode xmlNode = root.SelectSingleNode(baseurlexp);
                     XmlNode xmlNode1 = root.SelectSingleNode(copyrightexp);
                     XmlNode xmlNode2 = root.SelectSingleNode(dateexp);
                     string baseurl = xmlNode.InnerText;
                     string copyrighturl = xmlNode1.InnerText;
                     string date = xmlNode2.InnerText;
                     string plus = "." + baseurl.Split('.')[1];

                     BackgroundDownloader backgroundDownload = new BackgroundDownloader();

                     StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe\\BingPics", CreationCollisionOption.OpenIfExists);
                     StorageFile newFile = await folder.CreateFileAsync(date + plus, CreationCollisionOption.OpenIfExists);

                     Uri uri = new Uri(downtemp + baseurl);
                     DownloadOperation download = backgroundDownload.CreateDownload(uri, newFile);

                     await download.StartAsync();
                     return true;
                 }
                 catch
                 {
                     return false;
                 }
             });
        }
    }
}
