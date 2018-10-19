using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using αcafe.Info;
using αcafe.Functions;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.IO;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Threading.Tasks;
using αcafe.UzrControls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Upload_Page : Page
    {
        List<UploadImageInfo> imginfo = new List<UploadImageInfo>();
        ObservableCollection<UploadImageInfo> imgitems = new ObservableCollection<UploadImageInfo>();
        List<string> fileIdList = new List<string>();
        List<string> cameras = new List<string>();
        List<string> lens = new List<string>();
        TokenInfo tokenInfo = new TokenInfo();
        string userId;
        int i=0;

        public Upload_Page()
        {
            this.InitializeComponent();
            SelectedFilesListview.ItemsSource = imgitems;
        }

        private async void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            imginfo.Clear();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            var files = await openPicker.PickMultipleFilesAsync();
            TipsTb.Text = GetTips.GetRdTips();
            LoadingControl.IsLoading = true;

            foreach (StorageFile sf in files)
            {
                tokenInfo = await TokenInfo.GetTokenAsync();
                UploadImageInfo uploadImages = new UploadImageInfo();
                BitmapImage bitmap = new BitmapImage();
                using (var stream = await sf.OpenAsync(FileAccessMode.ReadWrite))
                {
                    bitmap.SetSource(stream);
                    if (stream.Size < 1024 * 1024)
                    {
                        uploadImages.DataSize = Math.Round((double)stream.Size / 1024, 2).ToString ()+"  Kb";
                    }
                    else
                    {
                        uploadImages.DataSize = Math.Round((double)stream.Size / 1024 / 1024, 2).ToString()+"  Mb";
                    }
                    Stream s = WindowsRuntimeStreamExtensions.AsStreamForRead(stream.GetInputStreamAt(0));
                    uploadImages.ContentByte = ConvertStreamTobyte(s);
                    s.Close();
                }
                uploadImages.Content = bitmap;
                uploadImages.Name = sf.Name;
                uploadImages.DateCreated = sf.DateCreated.ToString();
                uploadImages.Cameras = cameras;
                uploadImages.Lens = lens;
                imginfo.Add(uploadImages);
            }
            imginfo.ForEach(x => imgitems.Add(x));
            LoadingControl.IsLoading = false;
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = GetParentObject<ListViewItem>(((Button)sender),"");
            int b = Convert.ToInt32(imgitems.IndexOf((UploadImageInfo)listViewItem.Content));
            imgitems.RemoveAt(b);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            userId = LoginStatus.LoginInfo().userid;
            cameras = CameraNLensInfo.GetCNLList("cameras");
            lens = CameraNLensInfo.GetCNLList("lens");
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void animations_Click(object sender, RoutedEventArgs e)
        {
            PermissionDialog PD = new PermissionDialog();
            PD.PrimaryButtonClick += async (_s, _e) =>
            {
                List<RelativePanel> rps = GetChildObjects<RelativePanel>(SelectedFilesListview, "");
                foreach (RelativePanel rp in rps)
                {
                    ProgressRing pr = GetChildObject<ProgressRing>(rp, "");
                    DropShadowPanel dsp = GetChildObject<DropShadowPanel>(rp, "dropshadow");
                    DropShadowPanel dsp1 = GetChildObject<DropShadowPanel>(rp, "dropshadow1");
                    dsp1.Fade(0, 1000, 0).Start();
                    pr.IsActive = true;
                    await Task.Delay(2000);
                    pr.Fade(0, 1000, 0).Start();
                    dsp.Fade(1, 1000, 0).Start();
                }
            };
            await PD.ShowAsync();
        }

        public async void Upload()
        {
            foreach (UploadImageInfo item in imgitems)
            {
                string fileId = await UploadImageInfo.UploadFilesAsync(item, tokenInfo.userToken, tokenInfo.upLoadToken, userId, "16");
                if (fileId.Contains("failed") == false)
                {
                    item.FileID.Insert(0, fileId);
                }
            }
        }

        private void AutoSuggestBoxC_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ObservableCollection<String> rawsuggestions=new ObservableCollection<string>();
            ObservableCollection<String> suggestions = new ObservableCollection<string>();
            cameras.ForEach(x=>rawsuggestions.Add(x));

            foreach (string s in rawsuggestions)
            {
                if (s.Contains(sender.Text.ToLower ())==true|| s.Contains(sender.Text.ToUpper()) == true)
                {
                    suggestions.Add(s);
                }
            }
            sender.ItemsSource = suggestions;
        }

        private void AutoSuggestBoxL_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ObservableCollection<String> rawsuggestions = new ObservableCollection<string>();
            ObservableCollection<String> suggestions = new ObservableCollection<string>();
            lens.ForEach(x => rawsuggestions.Add(x));

            foreach (string s in rawsuggestions)
            {
                if (s.Contains(sender.Text.ToLower()) == true || s.Contains(sender.Text.ToUpper()) == true)
                {
                    suggestions.Add(s);
                }
            }
            sender.ItemsSource = suggestions;
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoreBtn.Tag.ToString() == "closed")
            {
                expand.Begin();
                DelAllBtn.Fade(1,1000,0).Start();
                DelAllBtn.Visibility = Visibility.Visible;
                ModBtn.Fade(1, 1000, 0).Start();
                ModBtn.Visibility = Visibility.Visible;
                TagSymbol.Symbol = Symbol.Up;
                MoreBtn.Tag = "opened";
            }
            else
            {
                close.Begin();
                DelAllBtn.Visibility = Visibility.Collapsed ;
                ModBtn.Visibility = Visibility.Collapsed ;
                TagSymbol.Symbol = Symbol.More;
                MoreBtn.Tag = "closed";
            }
        }

        private void DelAllBtn_Click(object sender, RoutedEventArgs e)
        {
            imginfo.Clear();
            imgitems.Clear();
        }

        public T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            int i = 0;
            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        public T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
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
