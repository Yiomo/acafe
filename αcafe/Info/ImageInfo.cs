using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using αcafe.Jsons;
using αcafe.Functions;

namespace αcafe.Info
{
    class ImageInfo
    {
        public static List<ImageInfo> GetImageInfo_P(string requrl, string referer)
        {
                string temp = requrl;
                HttpResponseMessage response;

                HttpClient request = new HttpClient();
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

                string b = textWriter.ToString().Replace("Exposure Time", "Exposure_Time");

                JsonImage. RootObject rb1 = JsonConvert.DeserializeObject<JsonImage.RootObject>(b);
                List<ImageInfo> imginfo = new List<ImageInfo>();
                List<string> fileID = new List<string>();
                List<string> filePath = new List<string>();
                List<string> fileStatus = new List<string>();
                List<string> likeNo = new List<string>();
                List<string> thumbImage = new List<string>();
                List<string> title = new List<string>();
                List<string> uploadTimeStamp = new List<string>();
                List<string> userID = new List<string>();
                List<string> viewCount = new List<string>();
                List<string> Model = new List<string>();
                List<string> Theme = new List<string>();
                List<string> Lens = new List<string>();
                List<string> Exposure_Time = new List<string>();
                List<string> FNumber = new List<string>();
                List<string> ISO = new List<string>();

                foreach (JsonImage.FileList fl in rb1.fileList)
                {
                    fileID.Add(fl.fileID);
                    filePath.Add(fl.filePath);
                    fileStatus.Add(fl.fileStatus);
                    likeNo.Add(fl.likeNo);
                    thumbImage.Add(fl.thumbImage);
                    title.Add(fl.title);
                    uploadTimeStamp.Add(fl.uploadTimeStamp);
                    userID.Add(fl.userID);
                    viewCount.Add(fl.viewCount);
                    JsonImage.DynamicInfo di = JsonConvert.DeserializeObject<JsonImage.DynamicInfo>(fl.dynamicInfo);
                    Model.Add(di.Model);
                    Lens.Add(di.Lens);
                    Theme.Add(di.Theme);
                    Exposure_Time.Add(di.Exposure_Time);
                    FNumber.Add(di.FNumber);
                    ISO.Add(di.ISO);
                }

                for (int i = 0; i < title.Count; i++)
                {
                    ImageInfo II = new ImageInfo
                    {
                        fileID = fileID[i],
                        filePath = filePath[i],
                        fileStatus = fileStatus[i],
                        likeNo = likeNo[i],
                        thumbImage = thumbImage[i],
                        title = title[i],
                        uploadTimeStamp = uploadTimeStamp[i],
                        userID = userID[i],
                        viewCount = viewCount[i],
                        Model = Model[i],
                        Lens = Lens[i],
                        Theme = Theme[i],
                        Exposure_Time = Exposure_Time[i],
                        FNumber = FNumber[i],
                        ISO = ISO[i]
                    };
                    imginfo.Add(II);
                }
            return imginfo;
        }

        public static Task<List<ImageInfo>> GetImageInfo_PT(string requrl, string referer)
        {
            return Task.Run(() => {
                try
                {
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

                    string b = textWriter.ToString().Replace("Exposure Time", "Exposure_Time");

                    JsonImage.RootObject rb1 = JsonConvert.DeserializeObject<JsonImage.RootObject>(b);
                    List<ImageInfo> imginfo = new List<ImageInfo>();
                    List<string> fileID = new List<string>();
                    List<string> filePath = new List<string>();
                    List<string> fileStatus = new List<string>();
                    List<string> likeNo = new List<string>();
                    List<string> thumbImage = new List<string>();
                    List<string> title = new List<string>();
                    List<string> uploadTimeStamp = new List<string>();
                    List<string> userID = new List<string>();
                    List<string> viewCount = new List<string>();
                    List<string> Model = new List<string>();
                    List<string> Theme = new List<string>();
                    List<string> Lens = new List<string>();
                    List<string> Exposure_Time = new List<string>();
                    List<string> FNumber = new List<string>();
                    List<string> ISO = new List<string>();

                    foreach (JsonImage.FileList fl in rb1.fileList)
                    {
                        fileID.Add(fl.fileID);
                        filePath.Add(fl.filePath);
                        fileStatus.Add(fl.fileStatus);
                        likeNo.Add(fl.likeNo);
                        thumbImage.Add(fl.thumbImage);
                        title.Add(fl.title);
                        uploadTimeStamp.Add(fl.uploadTimeStamp);
                        userID.Add(fl.userID);
                        viewCount.Add(fl.viewCount);
                        JsonImage.DynamicInfo di = JsonConvert.DeserializeObject<JsonImage.DynamicInfo>(fl.dynamicInfo);
                        Model.Add(di.Model);
                        Lens.Add(di.Lens);
                        Theme.Add(di.Theme);
                        Exposure_Time.Add(di.Exposure_Time);
                        FNumber.Add(di.FNumber);
                        ISO.Add(di.ISO);
                    }
                    for (int i = 0; i < title.Count; i++)
                    {
                        ImageInfo II = new ImageInfo
                        {
                            fileID = fileID[i],
                            filePath = filePath[i],
                            fileStatus = fileStatus[i],
                            likeNo = likeNo[i],
                            thumbImage = thumbImage[i],
                            title = title[i],
                            uploadTimeStamp = uploadTimeStamp[i],
                            userID = userID[i],
                            viewCount = viewCount[i],
                            Model = Model[i],
                            Lens = Lens[i],
                            Theme = Theme[i],
                            Exposure_Time = Exposure_Time[i],
                            FNumber = FNumber[i],
                            ISO = ISO[i]
                        };
                        imginfo.Add(II);
                    }
                    return imginfo;
                }
                catch
                {
                    return null;
                }
            });
        }

        public static Task<List<ImageInfo>> GetImageInfo_PT_Cookie(string requrl, string referer)
        {
            return Task.Run(() => {
                try
                {
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

                    string b = textWriter.ToString().Replace("Exposure Time", "Exposure_Time");

                    JsonImage.RootObject rb1 = JsonConvert.DeserializeObject<JsonImage.RootObject>(b);
                    List<ImageInfo> imginfo = new List<ImageInfo>();
                    List<string> fileID = new List<string>();
                    List<string> filePath = new List<string>();
                    List<string> fileStatus = new List<string>();
                    List<string> likeNo = new List<string>();
                    List<string> thumbImage = new List<string>();
                    List<string> title = new List<string>();
                    List<string> uploadTimeStamp = new List<string>();
                    List<string> userID = new List<string>();
                    List<string> viewCount = new List<string>();
                    List<string> Model = new List<string>();
                    List<string> Theme = new List<string>();
                    List<string> Lens = new List<string>();
                    List<string> Exposure_Time = new List<string>();
                    List<string> FNumber = new List<string>();
                    List<string> ISO = new List<string>();

                    foreach (JsonImage.FileList fl in rb1.fileList)
                    {
                        fileID.Add(fl.fileID);
                        filePath.Add(fl.filePath);
                        fileStatus.Add(fl.fileStatus);
                        likeNo.Add(fl.likeNo);
                        thumbImage.Add(fl.thumbImage);
                        title.Add(fl.title);
                        uploadTimeStamp.Add(fl.uploadTimeStamp);
                        userID.Add(fl.userID);
                        viewCount.Add(fl.viewCount);
                        JsonImage.DynamicInfo di = JsonConvert.DeserializeObject<JsonImage.DynamicInfo>(fl.dynamicInfo);
                        Model.Add(di.Model);
                        Lens.Add(di.Lens);
                        Theme.Add(di.Theme);
                        Exposure_Time.Add(di.Exposure_Time);
                        FNumber.Add(di.FNumber);
                        ISO.Add(di.ISO);
                    }
                    for (int i = 0; i < title.Count; i++)
                    {
                        ImageInfo II = new ImageInfo
                        {
                            fileID = fileID[i],
                            filePath = filePath[i],
                            fileStatus = fileStatus[i],
                            likeNo = likeNo[i],
                            thumbImage = thumbImage[i],
                            title = title[i],
                            uploadTimeStamp = uploadTimeStamp[i],
                            userID = userID[i],
                            viewCount = viewCount[i],
                            Model = Model[i],
                            Lens = Lens[i],
                            Theme = Theme[i],
                            Exposure_Time = Exposure_Time[i],
                            FNumber = FNumber[i],
                            ISO = ISO[i]
                        };
                        imginfo.Add(II);
                    }
                    return imginfo;
                }
                catch
                {
                    return null;
                }
            });
        }



        public string campaignID { get; set; }
        public string description { get; set; }
        public string dislikeNo { get; set; }
        public string fileID { get; set; }
        public string filePath { get; set; }
        public string fileStatus { get; set; }
        public string fileType { get; set; }
        public string likeNo { get; set; }
        public string thumbImage { get; set; }
        public string title { get; set; }
        public string uploadTimeStamp { get; set; }
        public string userID { get; set; }
        public string viewCount { get; set; }
        public string Model { get; set; }
        public string Theme { get; set; }
        public string Lens { get; set; }
        public string Exposure_Time { get; set; }
        public string FNumber { get; set; }
        public string ISO { get; set; }
    }
}
