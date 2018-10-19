using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using αcafe.Info;

namespace αcafe.Functions
{
    class ShutterCounter
    {
        public static async Task<string> ShutterCounterAsync(UploadImageInfo uploadImageInfo)
        {
            HttpClient httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            var contentByteContent = new ByteArrayContent(uploadImageInfo.ContentByte);
            HttpResponseMessage response = new HttpResponseMessage();

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.8,zh-TW;q=0.7,zh-HK;q=0.5,en-US;q=0.3,en;q=0.2");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.Add("Referer", "http://tools.science.si/index.php");
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");

            contentByteContent.Headers.Add("Content-Type", "image/jpeg");
            contentByteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            contentByteContent.Headers.ContentDisposition.FileName = "\"" + uploadImageInfo.Name + "\"";
            contentByteContent.Headers.ContentDisposition.Name = "\"filet\"";

            content.Add(new StringContent("ul"), "nacin");
            content.Add(new StringContent(""), "povezava");
            content.Add(contentByteContent, "imagefile");

            try
            {
                response = await httpClient.PostAsync(new Uri("http://tools.science.si/index.php", UriKind.Absolute), content);
                if (response.IsSuccessStatusCode)
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    string result = GZipDecom.GZipDecompress(stream);

                    if (result .Contains("sorry"))
                    {
                        return "无法识别";
                    }
                    else
                    {
                        string s = result.Split(":")[3].Split("<")[0];
                        string f = result.Split(":")[2].Split("file")[1];
                        string m = result.Split(":")[1].Split("<")[0];
                        return "根据文件"+f +"检测到机器"+m+"已使用快门" + s+"次。";
                    }
                }
                else
                {
                    return "传输错误";
                }
            }
            catch
            {
                return "传输错误";
            }
        }
    }
}
