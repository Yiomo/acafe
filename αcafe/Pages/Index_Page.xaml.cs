using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using HtmlAgilityPack;
using αcafe.UzrControls;
using αcafe.Info;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using System;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Index_Page : Page
    {
        public Index_Page()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        { 
            string base_url = "http://www.sonystyle.com.cn/mysony/acafe/";
            string referer = "http://www.sonystyle.com.cn/mysony/acafe/filter_all.htm";
            HtmlWeb web = new HtmlWeb();

            try
            {
                HtmlDocument index = web.Load("http://www.sonystyle.com.cn/mysony/acafe/index.htm");
                /*
                foreach (var index_img in index.DocumentNode.SelectNodes("//ul[@class='slides']/li/a/img"))
                {
                    Index_Carousel_url_temp.Add(base_url + index_img.GetAttributeValue("src", ""));
                }

                foreach (var index_img in index.DocumentNode.SelectNodes("//div[@class='txt_flex']/ul/li"))
                {
                    string temp = index_img.InnerHtml.Replace("<br>", "\n").TrimEnd('\n', ' ');
                    Index_Carousel_txt_temp.Add(temp);
                }
                */
                List<ImageInfo> fg = new List<ImageInfo>();
                fg = ImageInfo.GetImageInfo_P("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&fileStatus=3&fromNo=1&toNo=12&filterOption={\"Theme\":\"风光\"}", referer);
                List<ImageInfo> rx = new List<ImageInfo>();
                rx = ImageInfo.GetImageInfo_P("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&fileStatus=3&fromNo=1&toNo=12&filterOption={\"Theme\":[\"人像\"]}", referer);
                List<ImageInfo> rwjp = new List<ImageInfo>();
                rwjp = ImageInfo.GetImageInfo_P("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&fileStatus=3&fromNo=1&toNo=12&filterOption={\"Theme\":[\"人文街拍\"]}", referer);
                List<ImageInfo> wj = new List<ImageInfo>();
                wj = ImageInfo.GetImageInfo_P("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&campaignId=16&fileStatus=3&fromNo=1&toNo=12&filterOption={\"Theme\":[\"微距\"]}", referer);

                for (int i = 0; i < Index_Carousel_url_temp.Count; i++)
                {
                    Index_Carousel IF = new Index_Carousel
                    {
                        Index_Carousel_url = Index_Carousel_url_temp[i],
                        Index_Carousel_txt = Index_Carousel_txt_temp[i]
                    };
                    Index_Carousel_Info.Add(IF);
                }
                CarouselControl.ItemsSource = Index_Carousel_Info;
                PrimaryGridViewControl.ItemsSource = fg;
                SecondaryGridViewControl.ItemsSource = rx;
                ThirdlyGridViewControl.ItemsSource = rwjp;
                FourthlyGridViewControl.ItemsSource = wj;
            }
            catch
            {
                RefreshControl rc = new RefreshControl();
                rc.Tapped+=async (_s, _e) =>
                {
                    rc.Refreshing();
                    await Task.Delay(100);
                    Load();
                    grid.Children.Remove(rc);
                };
                grid.Children.Add(rc);
            }
        }

        public class Index_Carousel
        {
            public string Index_Carousel_url { get; set; }
            public string Index_Carousel_txt { get; set; }
        }

        private List<Index_Carousel> Index_Carousel_Info = new List<Index_Carousel>();
        private List<string> Index_Carousel_url_temp = new List<string>();
        private List<string> Index_Carousel_txt_temp = new List<string>();

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
            dsp.Scale(1.2f,1.2f,(float)dsp.ActualWidth/2,(float )dsp.ActualHeight/2,500,0 ).Start();
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
