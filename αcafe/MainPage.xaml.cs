using αcafe.Functions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System;
using Windows.Storage;
using αcafe.UzrControls;
using αcafe.Info;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace αcafe
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false  ;
            var appTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.ButtonBackgroundColor = Colors.White;
            if (Setting.IsFirstlyRun()==true)
            {
                Setting.InitializeSettings();
            }
            if (Setting.GetSettingValue("BackgroundSource").Contains("Bing")==true)
            {
                if (BingSuccess() == true)
                {
                    SettingBackground("Bing");
                    InitializeComponent();
                    RootFrame.Navigate(typeof(Pages.Index_Page));
                }
                else
                {               
                    InitializeComponent();
                    RootFrame.Navigate(typeof(Pages.Index_Page));
                }
            }
            else
            {
                try
                {
                    SettingBackground("Custom");
                }
                finally
                {
                    InitializeComponent();
                    RootFrame.Navigate(typeof(Pages.Index_Page));
                }
            }
        }

        private void NaviView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "Index":
                        RootFrame.Navigate(typeof(Pages.Index_Page));
                    break;

                    case "Filter":
                        RootFrame.Navigate(typeof(Pages.Filter_Page));
                    break;

                    case "Login":
                    {
                        var a = LoginStatus.LoginInfo();
                        bool b = a.IsLogin;
                        string c = a.userid;
                        
                        if (b == true)
                        {
                            RootFrame.Navigate(typeof(Pages.User_Page),c);
                        }else
                            RootFrame.Navigate(typeof(Pages.Login_Page));
                        break;
                    }

                    case "Upload":
                    {
                        RootFrame.Navigate(typeof(Pages.Upload_Page));
                        break;
                    }

                case "Class":
                    {
                        RootFrame.Navigate(typeof(Pages.Class_Page));
                        break;
                    }
            }
            if (item.Content.ToString().Contains("设置") == true)
            {
                RootFrame.Navigate(typeof(Pages.Setting_Page));
            }
        }

        public async void SettingBackground(string type)
        {
            if (type.Contains("Bing") == true)
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                StorageFolder folder = await KnownFolders.PicturesLibrary.GetFolderAsync("Acafe");
                StorageFolder folder1 = await folder.GetFolderAsync("BingPics");
                StorageFile pic = await folder1.GetFileAsync(date + ".jpg");
                using (var stream = await pic.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage img = new BitmapImage();
                    await img.SetSourceAsync(stream);
                    backgroundImage.Source = img;
                }
            }
            else
            {
                string[] path = Setting.GetSettingValue("BackgroundSource").Split("\\");
                string name = path[path.Length-1];
                StorageFolder folder = await KnownFolders.PicturesLibrary.GetFolderAsync("Acafe");
                StorageFolder folder1 = await folder.GetFolderAsync("CustomPics");
                StorageFile pic = await folder1.GetFileAsync(name);
                using (var stream = await pic.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage img = new BitmapImage();
                    await img.SetSourceAsync(stream);
                    backgroundImage.Source = img;
                }
            }
        }

        public bool  BingSuccess()
        {
            return BingPics.GetBingPics().Result;
        }

        public async Task<bool> IsExistedAsync()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            StorageFolder folder = await KnownFolders.PicturesLibrary.GetFolderAsync("Acafe");
            StorageFolder folder1 = await folder.GetFolderAsync("BingPics");
            try
            {
                StorageFile pic = await folder1.GetFileAsync(date + ".jpg");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void NavigationViewItem_Loading(Windows.UI.Xaml.FrameworkElement sender, object args)
        {
            ((NavigationViewItem)sender).IsSelected = true;
        }
    }
}
