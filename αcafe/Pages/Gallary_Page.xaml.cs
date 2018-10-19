using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using αcafe.Info;
using αcafe.UzrControls;
using αcafe.Functions;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Animations;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Gallary_Page : Page
    {
        public string baserequest = "http://www.sonystyle.com.cn/mysony/campaign/api/";
        public string basereferer = "http://www.sonystyle.com.cn/mysony/acafe/filter_all.htm";
        public string loadmore = "";
        public int loadcount = 0;
        ObservableCollection<ImageInfo> items = new ObservableCollection<ImageInfo>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Load(e);
        }

        public Gallary_Page()
        {
            this.InitializeComponent();
        }

        public async void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing pr = new ProgressRing()
            {
                IsActive = true,
            };

            LoadmoreBtn.Content = pr;
            loadcount += 1;
            int start = loadcount * 12 + 1;
            int end = 12 + loadcount * 12;
            string loadmore_temp = "fromNo=" + start + "&toNo=" + end;
            string Req_loadmore = loadmore.Replace("fromNo=1&toNo=12", loadmore_temp);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await GetListAsync(Req_loadmore, basereferer);
            });
        }

        public async void Load(NavigationEventArgs e)
        {
            try
            {
                items.Clear();
                List<ImageInfo> receiveIMGInfo = new List<ImageInfo>();
                if (e.Parameter != null)
                {
                    LoadingControl.IsLoading = true;
                    receiveIMGInfo = await ImageInfo.GetImageInfo_PT(e.Parameter.ToString(), basereferer);
                    receiveIMGInfo.ForEach(x => items.Add(x));
                    AdaptiveGridViewControl.ItemsSource = items;
                    LoadmoreBtn.Visibility = Visibility;
                    loadmore = e.Parameter.ToString();
                    loadcount = 0;
                    loadcountBK.Text = loadcount.ToString();
                    LoadingControl.IsLoading = false;
                }
                else
                {
                    LoadingControl.IsLoading = true;
                    receiveIMGInfo = await ImageInfo.GetImageInfo_PT("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&filterOption={}&fromNo=1&toNo=12", basereferer);
                    receiveIMGInfo.ForEach(x => items.Add(x));
                    AdaptiveGridViewControl.ItemsSource = items;
                    LoadmoreBtn.Visibility = Visibility;
                    loadmore = "http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&filterOption={}&fromNo=1&toNo=12";
                    loadcount = 0;
                    loadcountBK.Text = loadcount.ToString();
                    LoadingControl.IsLoading = false;
                }
            }
            catch
            {
                RefreshControl rc = new RefreshControl();
                rc.Tapped += async (_s, _e) =>
                {
                    rc.Refreshing();
                    await Task.Delay(100);
                    Load(e);
                    grid.Children.Remove(rc);
                };
                grid.Children.Add(rc);
            }
        }

        private async Task GetListAsync(string requrl, string referer)
        {
            List<ImageInfo> loadmoreIMGInfo = new List<ImageInfo>();
            loadmoreIMGInfo = await ImageInfo.GetImageInfo_PT(requrl, referer);
            loadmoreIMGInfo.ForEach(x => items.Add(x));
            AdaptiveGridViewControl.ItemsSource = items;
            loadcountBK.Text = loadcount.ToString();
            if (items.Count % 12 != 0)
            {
                LoadmoreBtn.Content = "没有更多啦~";
                LoadmoreBtn.IsEnabled = false;
            }
            else LoadmoreBtn.Content = "点击加载更多~";
        }

        private async void ImageEx_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            string fileid = (((ImageEx)sender).Tag).ToString();
            LoadingControl.IsLoading = true;
            List<RawInfo> rawinfo = await RawInfo.GetRawInfoAsync(fileid);
            Image_Detail IMD = new Image_Detail(rawinfo);
            LoadingControl.IsLoading = false;
            IMD.ShowAPopup();
        }

        private void ImageEx_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            DropShadowPanel dsp = GetParentObject<DropShadowPanel>(((ImageEx)sender), "");
            dsp.Scale(1.2f, 1.2f, (float)dsp.ActualWidth / 2, (float)dsp.ActualHeight / 2, 500, 0).Start();
        }

        private void ImageEx_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            DropShadowPanel dsp = GetParentObject<DropShadowPanel>(((ImageEx)sender), "");
            dsp.Scale(1f, 1f, (float)dsp.ActualWidth / 2, (float)dsp.ActualHeight / 2, 500, 0).Start();
        }

        private void ImageEx_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            DropShadowPanel dsp = GetParentObject<DropShadowPanel>(((ImageEx)sender), "");
            dsp.Scale(1f, 1f, (float)dsp.ActualWidth / 2, (float)dsp.ActualHeight / 2, 500, 0).Start();
        }


        private void LoadingContentControl_Loading(FrameworkElement sender, object args)
        {
            TipsTb.Text = GetTips.GetRdTips();
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

    }
}
