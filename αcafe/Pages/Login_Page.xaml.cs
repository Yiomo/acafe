using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using αcafe.Functions;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login_Page : Page
    {
        int a = 1;
        public Login_Page()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string baseReq = "http://www.sonystyle.com.cn/mysony/campaign/api/user.do?methodName=getUserDetails&keys=userId";
            HttpClient request = new HttpClient(Sign.HttpClientHandler);
            HttpResponseMessage response = request.GetAsync(baseReq).Result;
            string json = await response.Content.ReadAsStringAsync();

            if (json.Contains("userId"))
            {
                string[] temp;
                string userid;
                temp = (json.Split(':'))[1].Split(',');
                userid = temp[0].Replace('"', ' ');
                frame.Navigate(typeof(User_Page), userid);
            }
            else
            {
                string Req = "https://www.sonystyle.com.cn/mysony/campaign/api/captcha.do";
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await GetVerCodeAsync(Req);
                });
            }
        }

        private async void VerCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            VerCodeImg.Visibility = Visibility.Collapsed;
            a = a + 1;
            string plus = "?test=" + a;
            string baseReq = "https://www.sonystyle.com.cn/mysony/campaign/api/captcha.do" + plus;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await GetVerCodeAsync(baseReq);
            });
            VerCodeImg.Visibility = Visibility.Visible;
        }

        private async void Sign_Click(object sender, RoutedEventArgs e)
        {
            string post = "https://www.sonystyle.com.cn/mysony/campaign/api/secure/user.do?methodName=login&userId=" + IDBX.Text.Replace("@", "%40") + "&pwd=" + PWDBX.Password.ToString() + "&verficationCode=" + VERBX.Text;
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await SignInAsync(post);
            });
        }

        private async Task GetVerCodeAsync(string Req)
        {
            HttpResponseMessage response = await Sign.VerCode(Req);
            byte[] streamByte = response.Content.ReadAsByteArrayAsync().Result;
            MemoryStream ms = new MemoryStream(streamByte);
            BitmapImage img = new BitmapImage();
            await img.SetSourceAsync(ms.AsRandomAccessStream());
            VerCodeImg.Source = img;
        }

        private async Task SignInAsync(string Post)
        {
            HttpResponseMessage response = await Sign.SignIn(Post);
            Stream stream = await response.Content.ReadAsStreamAsync();
            string result = GZipDecom.GZipDecompress(stream);

            if (result.Contains("Password Wrong"))
            {
                LoginStatus.Text = "密码错误";
                PWDBX.Password = "";
                ButtonAutomationPeer peer = new ButtonAutomationPeer(VerCodeBtn);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
                LoginSymbol.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                LoginSymbol.Visibility = Visibility.Collapsed;
            }
            else if (result.Contains("Verification code incorrect"))
            {
                LoginStatus.Text = "验证码错误";
                VERBX.Text = "";
                ButtonAutomationPeer peer = new ButtonAutomationPeer(VerCodeBtn);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
                LoginSymbol.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                LoginSymbol.Visibility = Visibility.Collapsed;
            }
            else if (result.Contains("Mandatory filed is missing"))
            {
                LoginStatus.Text = "请补全信息";
                ButtonAutomationPeer peer = new ButtonAutomationPeer(VerCodeBtn);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
                LoginSymbol.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                LoginSymbol.Visibility = Visibility.Collapsed;
            }
            else if (result.Contains("User Name or Email does not exist"))
            {
                LoginStatus.Text = "用户不存在";
                IDBX.Text = "";
                ButtonAutomationPeer peer = new ButtonAutomationPeer(VerCodeBtn);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
                LoginSymbol.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                LoginSymbol.Visibility = Visibility.Collapsed;
            }
            else
            {
                await Task.Delay(1000);
                string baseReq = "http://www.sonystyle.com.cn/mysony/campaign/api/user.do?methodName=getUserDetails&keys=userId";
                HttpClient request = new HttpClient(Sign.HttpClientHandler);
                HttpResponseMessage resp = request.GetAsync(baseReq).Result;
                string json = await resp.Content.ReadAsStringAsync();
                string[] temp;
                string userid;
                temp = (json.Split(':'))[1].Split(',');
                userid = temp[0].Replace('"', ' ');
                frame.Navigate(typeof(User_Page), userid);
            }
        }
    }
}
