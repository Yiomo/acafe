using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using αcafe.Info;
using αcafe.Functions;
using Windows.UI.ViewManagement;
using Windows.Storage;
using System;
using Windows.System.UserProfile;
using Windows.ApplicationModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace αcafe.UzrControls
{
    public sealed partial class Image_Detail : UserControl
    {
        private List<RawInfo> _popupContent;
        private Popup _popup = null;
        List<ImageInfo> imageInfo = new List<ImageInfo>();
        ObservableCollection<RawInfo> items = new ObservableCollection<RawInfo>();
        public int index=0;

        public Image_Detail()
        {
            this.InitializeComponent();
            _popup = new Popup();
            ApplicationView.GetForCurrentView().VisibleBoundsChanged += (s, e) =>
            {
                MeasurePopupSize();
            };
            MeasurePopupSize();
            _popup.Child = this;

            this.Loaded += PopupNoticeLoaded;
        }

        public Image_Detail(List<RawInfo> rawinfo) : this()
        {
            _popupContent = rawinfo;
        }

        public void ShowAPopup()
        {
            _popup.IsOpen = true;
        }

        public async void PopupNoticeLoaded(object sender, RoutedEventArgs e)
        {
            string userid = _popupContent[0].userID;
            List<ImageInfo> temp = await ImageInfo.GetImageInfo_PT("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&fileStatus=1&filterOption={}&campaignID=16&userId="+userid, "http://www.sonystyle.com.cn/mysony/acafe/filter_all.htm");
            var temps = temp.GroupBy(x => x.filePath).Where(x=>x.Count()>=1).ToList();
            foreach (var item in temps)
            {
                imageInfo.Add(item.ToList()[0]);
            }
            items.Add(_popupContent[0]);
            for (int i = 0; i <= 2; i++)
            {
                List< RawInfo> rawInfo= await RawInfo.GetRawInfoAsync(imageInfo[i].fileID);
                if(rawInfo.Contains(_popupContent[0])==false )
                    rawInfo.ForEach(x => items.Add(x));
            }
            flipview.ItemsSource = items;
        }

        private void MeasurePopupSize()
        {
            this.Width = ApplicationView.GetForCurrentView().VisibleBounds.Width;
            this.Height = ApplicationView.GetForCurrentView().VisibleBounds.Height;
        }

        private async void flipview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = ((FlipView)sender).SelectedIndex;
            if (index==items.Count-1)
            {
                for (int i = index  ; i <index +1; i++)
                {
                    try
                    {
                        List<RawInfo> rawInfo = await RawInfo.GetRawInfoAsync(imageInfo[i].fileID);
                        if (rawInfo.Contains(_popupContent[0]) == false)
                            rawInfo.ForEach(x => items.Add(x));
                    }
                    catch
                    { }
                }
            }
        }

        private void ImageEx_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = GetParentObject<Grid>((ImageEx)sender,"");
            CommandBar CB = GetChildObject<CommandBar>(grid,"");
            FlipViewItem flipViewItem = GetParentObject<FlipViewItem>(grid, "");
            FlipView flipView = GetParentObject<FlipView>(flipViewItem,"");
            List<FlipViewItem> flipviewitems = GetChildObjects<FlipViewItem>(flipview, "");

            if (CB .Opacity == 0)
            {
                foreach (FlipViewItem item in flipviewitems)
                {
                    Grid grid1 = GetChildObject<Grid>(item, "grid");
                    CommandBar commandBar = GetChildObject<CommandBar>(grid1, "");
                    Border border = GetChildObject<Border>(grid1, "TitleBar");
                    SplitView splitView = GetChildObject<SplitView>(grid1, "");
                    commandBar.Fade(1f, 500, 0).Start();
                    border.Fade(1f, 500, 0).Start();
                }
            }
            else
            {
                foreach (FlipViewItem item in flipviewitems)
                {
                    Grid grid1 = GetChildObject<Grid>(item, "grid");
                    CommandBar commandBar = GetChildObject<CommandBar>(grid1, "");
                    Border border = GetChildObject<Border>(grid1, "TitleBar");
                    SplitView splitView = GetChildObject<SplitView>(grid1, "");
                    commandBar.Fade(0f, 500, 0).Start();
                    border.Fade(0f, 500, 0).Start();
                }
            }
        }

        private void Like_Click(object sender, RoutedEventArgs e)
        {
            string fileid = items[index].fileId;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            HttpClient request = new HttpClient(httpClientHandler);
            HttpResponseMessage response = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=updateLikeDislike&option=1&fileId=" + fileid).Result;
            string[] result = response.Content.ReadAsStringAsync().Result.Split(':');
            string likeno = result.Last().Replace('}', ' ');
            ((AppBarButton)sender).Label = likeno;
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            DownloadProperties DP = await GetDownloadProperties();
            if (DP == null)
            {
                DownloadSymbol.Visibility = Visibility.Visible;
                DownloadStatus.Text = "文件已存在";
                await Task.Delay(800);
                DownloadSymbol.Visibility = Visibility.Collapsed;
            }
            else
            {
                DownloadSymbol.Visibility = Visibility.Visible;
                string uri = DP.uri;
                string name = DP.name;
                DownloadStatus.Text = "下载中";
                await Task.Run(async () => { await OperateFile.SaveImageAsync(name, uri); });
                DownloadStatus.Text = "下载完成";
                await Task.Delay(800);
                DownloadSymbol.Visibility = Visibility.Collapsed;
            }
        }

        private void Detail_Open_Click(object sender, RoutedEventArgs e)
        {
            CommandBar commandBar = GetParentObject<CommandBar>((AppBarButton)sender, "");
            Grid grid = GetParentObject<Grid>(commandBar, "grid");
            SplitView splitView = GetChildObject<SplitView>(grid, "Detailarea");
            Pivot pivot = GetChildObject<Pivot>(splitView, "");
            PivotItem pivotItem = GetChildObject<PivotItem>(pivot, "CommsPivotItem");
            Grid grid2 = GetChildObject<Grid>(pivotItem, "");
            ContentPresenter contentPresenter = GetChildObject<ContentPresenter>(grid2, "");
            StackPanel stackPanel = (StackPanel)contentPresenter.Content;
            ListView listView = GetChildObject<ListView>(stackPanel, "");
            if (items[index].allComs.Count != 0)
            {
                listView.ItemsSource = items[index].allComs;
                splitView.IsPaneOpen = !splitView.IsPaneOpen;
            }
            else
                splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private async void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = GetParentObject<StackPanel>((Button)sender, "");
            TextBox textBox = GetChildObject<TextBox>(stackPanel,"");
            string comm = textBox.Text;

            HttpClient request = new HttpClient(Sign.HttpClientHandler);
            HttpResponseMessage response = request.GetAsync("http://www.sonystyle.com.cn/mysony/campaign/api/fileComment.do?methodName=insertComment&programId=1&fileId=" + items[index].fileId + "&comment=" + comm).Result;
            string result = await response.Content.ReadAsStringAsync();
            if (result.Contains("Success"))
            {
                textBox.Text = "";
                textBox.PlaceholderText = "发布成功，等待审核。";
            }
        }

        private async void BackTo_Click(object sender, RoutedEventArgs e)
        {
            await this.Fade(0f, 500, 0).StartAsync();
            _popup.IsOpen = false;
        }

        private void SetToStartScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            SetToScreen("start");
        }

        private void SetToLockScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            SetToScreen("lock");
        }

        private void Detailarea_LostFocus(object sender, RoutedEventArgs e)
        {
            ((SplitView)sender).IsPaneOpen = false;
        }

        private async Task<DownloadProperties> GetDownloadProperties()
        {
            string uri = "";
            string[] temp;
            string name = "";
            StorageFolder folder;
            StorageFile file;

            if (Setting.GetSettingValue("DownloadPath").Contains("图片"))
                folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe", CreationCollisionOption.OpenIfExists);
            else
                folder = await StorageFolder.GetFolderFromPathAsync(Setting.GetSettingValue("DownloadPath"));

            if (Setting.GetSettingValue("DownloadQuality").Contains("Raw") == true)
            {
                uri = items[index].fileUploadPath;
                temp = uri.Split('.');
                name = items[index].title + "R" + "." + temp[temp.Count() - 1];
                try
                {
                    file = await folder.GetFileAsync(name);
                    return null;
                }
                catch
                {
                    DownloadProperties downloadProperties = new DownloadProperties()
                    {
                        name = name,
                        uri = uri,
                    };
                    return downloadProperties;
                }
            }
            else
            {
                uri = items[index].filePath;
                temp = uri.Split('.');
                name = items[index].title + "N" + "." + temp[temp.Count() - 1];
                try
                {
                    file = await folder.GetFileAsync(name);
                    return null;
                }
                catch
                {
                    DownloadProperties downloadProperties = new DownloadProperties()
                    {
                        name = name,
                        uri = uri,
                    };
                    return downloadProperties;
                }
            }
        }

        private async void SetToScreen(string screen)
        {
            if (!UserProfilePersonalizationSettings.IsSupported())
            {
                DownloadSymbol.Visibility = Visibility.Visible;
                DownloadStatus.Text = "不支持哦";
                await Task.Delay(800);
                DownloadSymbol.Visibility = Visibility.Collapsed;
            }
            else
            {
                string uri = items[index].fileUploadPath;
                string[] temp = uri.Split('.');
                StorageFolder folder;
                if (Setting.GetSettingValue("DownloadPath").Contains("图片"))
                    folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe", CreationCollisionOption.OpenIfExists);
                else
                    folder = await StorageFolder.GetFolderFromPathAsync(Setting.GetSettingValue("DownloadPath")); ;
                StorageFile file;

                if (await IsRawExisted() == true)
                {
                    DownloadSymbol.Visibility = Visibility.Visible;
                    DownloadStatus.Text = "设置中";

                    StorageFolder base_folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                    file = await folder.GetFileAsync(items[index ].title + "R" + "." + temp[temp.Count() - 1]);
                    await file.CopyAsync(base_folder,items[index ].title + "R" + "." + temp[temp.Count() - 1], NameCollisionOption.ReplaceExisting);
                    StorageFile copy_file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/" + file.Name));

                    UserProfilePersonalizationSettings setting = UserProfilePersonalizationSettings.Current;
                    bool a;
                    if (screen.Contains("start"))
                        a = await setting.TrySetWallpaperImageAsync(copy_file);
                    else
                        a = await setting.TrySetLockScreenImageAsync(copy_file);
                    switch (a)
                    {
                        case true:
                            {
                                DownloadStatus.Text = "设置成功";
                                await Task.Delay(800);
                                DownloadSymbol.Visibility = Visibility.Collapsed;
                                break;
                            }
                        case false:
                            {
                                DownloadStatus.Text = "设置失败";
                                await Task.Delay(800);
                                DownloadSymbol.Visibility = Visibility.Collapsed;
                                break;
                            }
                    }
                }
                else
                {
                    string name = items[index].title + "R" + "." + temp[temp.Count() - 1];
                    DownloadSymbol.Visibility = Visibility.Visible;
                    DownloadStatus.Text = "下载中";
                    await Task.Run(async () => { await OperateFile.SaveImageAsync(name, uri); });
                    file = await folder.GetFileAsync(items[index].title + "R" + "." + temp[temp.Count() - 1]);
                    StorageFolder base_folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                    await file.CopyAsync(base_folder, items[index].title + "R" + "." + temp[temp.Count() - 1], NameCollisionOption.ReplaceExisting);
                    StorageFile copy_file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/" + file.Name));

                    UserProfilePersonalizationSettings setting = UserProfilePersonalizationSettings.Current;
                    bool a;
                    if (screen.Contains("start"))
                        a = await setting.TrySetWallpaperImageAsync(copy_file);
                    else
                        a = await setting.TrySetLockScreenImageAsync(copy_file);

                    switch (a)
                    {
                        case true:
                            {
                                DownloadStatus.Text = "设置成功";
                                await Task.Delay(800);
                                DownloadSymbol.Visibility = Visibility.Collapsed;
                                break;
                            }
                        case false:
                            {
                                DownloadStatus.Text = "设置失败";
                                await Task.Delay(800);
                                DownloadSymbol.Visibility = Visibility.Collapsed;
                                break;
                            }
                    }
                }
            }
        }

        private async Task<bool> IsRawExisted()
        {
            string uri = "";
            string[] temp;
            string name = "";
            StorageFolder folder;
            StorageFile file;

            if (Setting.GetSettingValue("DownloadPath").Contains("图片"))
                folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Acafe", CreationCollisionOption.OpenIfExists);
            else
                folder = await StorageFolder.GetFolderFromPathAsync(Setting.GetSettingValue("DownloadPath")); ;

            uri =items[index].fileUploadPath;
            temp = uri.Split('.');
            name = items[index ].title + "R" + "." + temp[temp.Count() - 1];
            try
            {
                file = await folder.GetFileAsync(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public class DownloadProperties
        {
            public string name { get; set; }
            public string uri { get; set; }
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
    }
}
