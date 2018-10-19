using System;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace αcafe.Functions
{
    class OperateFile
    {
        public static  async Task SaveImageAsync(string imageName, string imageUri)
        {
            BackgroundDownloader backgroundDownload = new BackgroundDownloader();
            StorageFolder folder =null  ;
            StorageFile newFile=null ;

            if (Setting.GetSettingValue("DownloadPath").Contains("我的文档\\图片\\Acafe"))
            {
                folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe", CreationCollisionOption.OpenIfExists);
                newFile = await folder.CreateFileAsync(imageName, CreationCollisionOption.OpenIfExists);
            }
            else
            {
                folder = await StorageFolder.GetFolderFromPathAsync(Setting.GetSettingValue("DownloadPath"));
                newFile = await folder.CreateFileAsync(imageName, CreationCollisionOption.OpenIfExists);
            }

            Uri uri = new Uri(imageUri);
            DownloadOperation download = backgroundDownload.CreateDownload(uri, newFile);

            await download.StartAsync();

        }

        public static async Task SetToScreenAsync(string imageName, string imageUri)
        {
            BackgroundDownloader backgroundDownload = new BackgroundDownloader();
            StorageFolder folder = null;
            StorageFile newFile = null;

            if (Setting.GetSettingValue("DownloadPath").Contains("我的文档\\图片\\Acafe"))
            {
                folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe", CreationCollisionOption.OpenIfExists);
                newFile = await folder.CreateFileAsync(imageName, CreationCollisionOption.OpenIfExists);
            }
            else
            {
                folder = await StorageFolder.GetFolderFromPathAsync(Setting.GetSettingValue("DownloadPath"));
                newFile = await folder.CreateFileAsync(imageName, CreationCollisionOption.OpenIfExists);
            }

            Uri uri = new Uri(imageUri);
            DownloadOperation download = backgroundDownload.CreateDownload(uri, newFile);

            await download.StartAsync();
        }
    }
}
