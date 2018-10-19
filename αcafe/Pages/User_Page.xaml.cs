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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class User_Page : Page
    {
        string basereq = "http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileList&fileStatus=1&campaignId=16&fromNo=1&toNo=12&userId=";
        string basereferer = "http://www.sonystyle.com.cn/my…ny/acafe/myspace/homepage.htm";
        string camreq = "http://www.sonystyle.com.cn/mysony/campaign/api/campaign.do?methodName=getMyCampaignParticipationList&programId=1&userId=";
        public string loadmore = "";
        public int loadcount = 0;
        ObservableCollection<ImageInfo> items = new ObservableCollection<ImageInfo>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            useridTb.Text = e.Parameter.ToString();
            List<ImageInfo> receiveIMGInfo = new List<ImageInfo>();
            List<CampaignInfo> campaignInfo = new List<CampaignInfo>();
            List<CommentsInfo> commentsInInfo = new List<CommentsInfo>();
            List<CommentsInfo> commentsOutInfo = new List<CommentsInfo>();
            List<FollowerInfo> followerInfo = new List<FollowerInfo>();
            List<FollowerInfo> IamfollowerInfo = new List<FollowerInfo>();
            List<UserInfo> modelInfo = new List<UserInfo>();
            List<UserInfo> lensInfo = new List<UserInfo>();
            List<UserInfo> themeInfo = new List<UserInfo>();

            campaignInfo = await CampaignInfo.GetCampaignInfo_PT(camreq+e.Parameter.ToString (),basereferer);
            receiveIMGInfo = await ImageInfo.GetImageInfo_PT_Cookie(basereq+e.Parameter.ToString (), basereferer);
            receiveIMGInfo.ForEach(x => items.Add(x));
            LoadmoreBtn.Visibility = Visibility.Visible;
            commentsInInfo = await CommentsInfo.GetComInAsync();
            commentsOutInfo = await CommentsInfo.GetComOutAsync();
            modelInfo = await UserInfo.GetUserInfo_PT("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileCountByGroup&filterOption=Model&programId=1&userId="+e.Parameter.ToString (),basereferer);
            lensInfo = await UserInfo.GetUserInfo_PT("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileCountByGroup&filterOption=Lens&programId=1&userId=" + e.Parameter.ToString (),basereferer);
            themeInfo = await UserInfo.GetUserInfo_PT("http://www.sonystyle.com.cn/mysony/campaign/api/file.do?methodName=getFileCountByGroup&filterOption=Theme&programId=1&userId="+e.Parameter.ToString(),basereferer);
            followerInfo = await FollowerInfo.GetMyFollowersAsync(e.Parameter.ToString());
            IamfollowerInfo = await FollowerInfo.GetIamFollowersAsync(e.Parameter.ToString());
            UserPicAdaGridview.ItemsSource = items;
            CamListview.ItemsSource = campaignInfo;
            ComInListview.ItemsSource = commentsInInfo;
            ComOutListview.ItemsSource = commentsOutInfo;
            MyFollowersListView.ItemsSource = followerInfo;
            IamFollowersListView.ItemsSource = IamfollowerInfo;
        }

        public User_Page()
        {
            this.InitializeComponent();
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
            string Req_loadmore = basereq.Replace("fromNo=1&toNo=12", loadmore_temp)+useridTb.Text;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await GetListAsync(Req_loadmore, basereferer);
            });
        }

        private async Task GetListAsync(string requrl, string referer)
        {
            List<ImageInfo> loadmoreIMGInfo = new List<ImageInfo>();
            loadmoreIMGInfo = await ImageInfo.GetImageInfo_PT(requrl, referer);
            loadmoreIMGInfo.ForEach(x => items.Add(x));
            UserPicAdaGridview.ItemsSource = items;
            loadcountBK.Text = loadcount.ToString();
            if (items.Count % 12 != 0)
            {
                LoadmoreBtn.Content = "没有更多啦~";
                LoadmoreBtn.IsEnabled = false;
            }
            else LoadmoreBtn.Content = "点击加载更多~";
        }
    }
}
