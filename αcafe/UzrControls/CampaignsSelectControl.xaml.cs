using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using αcafe.Info;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace αcafe.UzrControls
{
    public sealed partial class CampaignsSelectControl : ContentDialog
    {
        public CampaignsSelectControl()
        {
            this.InitializeComponent();
            Load();
        }

        public async void Load()
        {
            List<CampaignInfo> campaignInfo = await CampaignInfo.GetNowCampaignInfo();
            CampaignGridview.ItemsSource = campaignInfo;
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void CampaignGridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            int a = 1;
        }
    }
}
