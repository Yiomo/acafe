using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using αcafe.Functions;
using αcafe.Info;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Setting_Page : Page
    {
        UploadImageInfo uploadImages = new UploadImageInfo();

        public Setting_Page()
        {
            this.InitializeComponent();
            if (Setting.GetSettingValue("BackgroundSource").Contains("Bing") == true)
                bingswitch.IsOn = false;
            else
            {
                bingswitch.IsOn = true;
                picpathtb.Text = Setting.GetSettingValue("BackgroundSource");
                picpathtb.Visibility = Visibility.Visible;
            }

            switch (Setting.GetSettingValue("DownloadQuality"))
            {
                case "Normal":
                    selBtn.Content = "普通";
                    break;
                case "Raw":
                    selBtn.Content = "原图";
                    break;
            }

            downloadpathTb.Text = Setting.GetSettingValue("DownloadPath");
        }

        private async void Openpic_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var pic = await openPicker.PickSingleFileAsync();
            picpathtb.Text = pic.Path;
            picpathtb.Visibility = Visibility.Visible;
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe\\CustomPics", CreationCollisionOption.OpenIfExists);
            await pic.CopyAsync(folder,pic.Name);
            Setting.SetSettingValue("BackgroundSource",pic.Path);
        }

        private async void Opendownload_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            downloadpathTb.Text = folder.Path;
            Setting.SetSettingValue("DownloadPath", folder.Path);
        }

        private void normalBtn_Click(object sender, RoutedEventArgs e)
        {
            selBtn.Content = ((MenuFlyoutItem)sender).Text;
            Setting.SetSettingValue("DownloadQuality","Normal");
        }

        private void rawBtn_Click(object sender, RoutedEventArgs e)
        {
            selBtn.Content = ((MenuFlyoutItem)sender).Text;
            Setting.SetSettingValue("DownloadQuality", "Raw");
        }

        private void bingswitch_Toggled(object sender, RoutedEventArgs e)
        {
            switch (bingswitch.IsOn)
            {
                case false :
                    Setting.SetSettingValue("BackgroundSource", "Bing");
                    picpathtb.Text = "";
                    picpathtb.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private async void FeedBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EmailRecipient emailRecipient1 = new EmailRecipient("Snowysong@Live.com");
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.To.Add(emailRecipient1);
            emailMessage.Subject = "αcafe使用反馈";
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private async void upbtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".JPG");
            openPicker.FileTypeFilter.Add(".ARW");

            var file = await openPicker.PickSingleFileAsync();
            tbk.Visibility = Visibility.Collapsed;
            skp.Visibility = Visibility.Visible;
            upbtn.IsEnabled = false;

            BitmapImage bitmap = new BitmapImage();

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                bitmap.SetSource(stream);
                if (stream.Size < 1024 * 1024)
                {
                    uploadImages.DataSize = Math.Round((double)stream.Size / 1024, 2).ToString() + "  Kb";
                }
                else
                {
                    uploadImages.DataSize = Math.Round((double)stream.Size / 1024 / 1024, 2).ToString() + "  Mb";
                }
                Stream s = WindowsRuntimeStreamExtensions.AsStreamForRead(stream.GetInputStreamAt(0));
                uploadImages.ContentByte = ConvertStreamTobyte(s);
                s.Close();
            }

            uploadImages.Content = bitmap;
            uploadImages.Name = file.Name;
            Upload();
        }

        public async void Upload()
        {
            string status = await ShutterCounter.ShutterCounterAsync(uploadImages);
            resulttbk.Text = status;
            tbk.Visibility = Visibility.Visible ;
            skp.Visibility = Visibility.Collapsed;
            upbtn.IsEnabled = true;
        }

        public static byte[] ConvertStreamTobyte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
