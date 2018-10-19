using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using αcafe.Functions;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http.Headers;
using System.ComponentModel;
using System.Collections.Generic;

namespace αcafe.Info
{
    class UploadImageInfo
    {
        public static async Task<string> UploadFilesAsync(UploadImageInfo uploadImageInfo, string uzrtoken, string uploadtoken, string userId, string campaignId)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(Sign.HttpClientHandler);
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/*");
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Shockwave Flash");

            HttpResponseMessage response = new HttpResponseMessage();
            var content = new MultipartFormDataContent();
            var contentByteContent = new ByteArrayContent(uploadImageInfo.ContentByte);
            contentByteContent.Headers.Add("Content-Type", "application/octet-stream");
            contentByteContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            contentByteContent.Headers.ContentDisposition.FileName = "\"" + uploadImageInfo.Name + "\"";
            contentByteContent.Headers.ContentDisposition.Name = "\"imagefile\"";
            {
                content.Add(new StringContent(uploadImageInfo.Name), "\"Filename\"");
                content.Add(new StringContent("1"), "\"uploadTokenType\"");
                content.Add(new StringContent("0"), "\"fileType\"");
                content.Add(new StringContent(uploadImageInfo.Name), "\"title\"");
                content.Add(new StringContent("2"), "\"userTokenType\"");
                content.Add(new StringContent(campaignId), "\"campaignId\"");
                content.Add(new StringContent(userId), "\"userId\"");
                content.Add(new StringContent(uploadtoken), "\"uploadToken\"");
                content.Add(new StringContent(uzrtoken), "\"userToken\"");
                content.Add(contentByteContent, "\"imagefile\"");
                content.Add(new StringContent("Submit Query"), "\"Upload\"");
            }

            response = await httpClient.PostAsync(new Uri("http://media.sony.com.cn/alphacafe/upload", UriKind.Absolute), content);
            string sid = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode&&sid.Contains("fileID")==true)
            {
                string s = response.Content.ReadAsStringAsync().Result.Split('"')[7];
                return s;
            }
            else return "failed";
        }

        //private void UploadFilesAsync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //}

        public string Name { get; set; }
        public string FileID { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public string DateCreated { get; set; }
        public string DataSize { get; set; }
        public BitmapImage Content { get; set; }
        public Byte[] ContentByte{ get; set; }
        public List<string> Cameras { get; set; }
        public List<string> Lens { get; set; }
    }
}
