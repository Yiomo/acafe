using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace αcafe.UzrControls
{
    public sealed partial class RefreshControl : UserControl
    {
        public RefreshControl()
        {
            this.InitializeComponent();
        }

        public void Refreshing()
        {
            pr.IsActive = true;
            statusTb.Text = "Loading...🧐";
        }
    }
}
