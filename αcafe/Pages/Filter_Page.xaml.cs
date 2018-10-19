using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using αcafe.Functions;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace αcafe.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Filter_Page : Page
    {
        string baseurl = "http://www.sonystyle.com.cn/mysony/campaign/api";
        string referer = "http://www.sonystyle.com.cn/mysony/acafe/filter_all.htm";
        string requesturl1 = "http://www.sonystyle.com.cn/mysony/acafe/xml/lens.xml";
        string requesturl2 = "http://www.sonystyle.com.cn/mysony/acafe/xml/camera.xml";
        string taglabel;
        public int a = 0;
        string sort = "";

        public Filter_Page()
        {
            this.InitializeComponent();
            GalleryFrame.Navigate(typeof(Gallary_Page));
        }

        private void ListViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            filterpane.IsPaneOpen = !filterpane.IsPaneOpen;
            if (((ListViewItem)sender).Tag.ToString() == "camera")
            {
                camera_selected.Text = ((ListViewItem)sender).Content.ToString();
                FilterBtn.IsEnabled = true;
            }
            else
            {
                lens_selected.Text = ((ListViewItem)sender).Content.ToString();
                FilterBtn.IsEnabled = true;
            }
            filterlistview.Items.Clear();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            filterlistview.Items.Clear();
            filterpane.IsPaneOpen = !filterpane.IsPaneOpen;
            string series_temp = ((MenuFlyoutItem)sender).Text.ToString();
            string category_temp = ((MenuFlyoutItem)sender).Tag.ToString();
            List<string> result_temp;

            if (category_temp == "camera")
            {
                category_temp = ((MenuFlyoutItem)sender).Text.ToString();
                result_temp = HttpXml.Responsetext(requesturl2, category_temp, null, "camera");
                taglabel = "camera"; 
            }
            else
            {
                result_temp = Functions.HttpXml.Responsetext(requesturl1, category_temp, series_temp, "lens");
                taglabel = "lens";
            }

            foreach (string temp in result_temp)
            {
                ListViewItem listViewItem = new ListViewItem
                {
                    Content = temp,
                    Tag = taglabel,
                };
                listViewItem.Tapped += ListViewItem_Tapped;
                filterlistview.Items.Add(listViewItem);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuFlyoutItem)sender).Tag.ToString() == "camera")
            {
                camera_selected.Text = "";
            }
            else
            {
                lens_selected.Text = "";
            }
            if (camera_selected.Text != "" || lens_selected.Text != "")
            {
                FilterBtn.IsEnabled = true;
            }
            else { FilterBtn.IsEnabled = false; }
        }

        private void ThemeSelectorReset_Click(object sender, RoutedEventArgs e)
        {
            a = 0;
            foreach (var items in ThemeSelector.Items)
            {
                if (items.GetType().ToString() == typeof(ToggleMenuFlyoutItem).ToString())
                {
                    ToggleMenuFlyoutItem tmf = (ToggleMenuFlyoutItem)items;
                    tmf.IsChecked = false;
                    tmf.IsEnabled = true;
                }
            }
            abk.Text = a.ToString();
            if (camera_selected.Text != "" || lens_selected.Text != "")
            {
                FilterBtn.IsEnabled = true;
            }
            else
                FilterBtn.IsEnabled = false;
        }

        private void ThemeSelector_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleMenuFlyoutItem)sender).IsChecked == true)
            {
                a = a + 1;
                if (a == 3)
                {
                    foreach (var items in ThemeSelector.Items)
                    {
                        if (items.GetType().ToString() == typeof(ToggleMenuFlyoutItem).ToString())
                        {
                            ToggleMenuFlyoutItem tmf = (ToggleMenuFlyoutItem)items;
                            if (tmf.IsChecked == false)
                            {
                                tmf.IsEnabled = false;
                            }
                        }
                    }
                }
            }
            else
            {
                a = a - 1;
                foreach (var items in ThemeSelector.Items)
                {
                    if (items.GetType().ToString() == typeof(ToggleMenuFlyoutItem).ToString())
                    {
                        ToggleMenuFlyoutItem tmf = (ToggleMenuFlyoutItem)items;
                        if (tmf.IsChecked == false)
                        {
                            tmf.IsEnabled = true;
                        }
                    }
                }

            }

            abk.Text = a.ToString();
            if (camera_selected.Text != "" || lens_selected.Text != "")
            {
                FilterBtn.IsEnabled = true;
            }
            else
                FilterBtn.IsEnabled = false;
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            string tag = ((MenuFlyoutItem)sender).Tag.ToString();
            switch (tag)
            {
                case "time": sort = ""; break;
                case "view": sort = "view"; break;
                case "like": sort = "like"; break;
            }
        }

        private async void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            string model;
            string lens;
            if (camera_selected.Text != "")
            {
                model = camera_selected.Text;
            }
            else model = null;
            if (lens_selected.Text != "")
            {
                lens = lens_selected.Text;
            }
            else lens = null;

            List<string> themes = new List<string>();
            if (a != 0)
            {
                foreach (var items in ThemeSelector.Items)
                {
                    if (items.GetType().ToString() == typeof(ToggleMenuFlyoutItem).ToString())
                    {
                        ToggleMenuFlyoutItem tmf = (ToggleMenuFlyoutItem)items;
                        if (tmf.IsChecked == true)
                            themes.Add(tmf.Text);
                    }
                }
            }
            else { themes = null; }

            string ReqUrl = CombineUrl.CombineRequest(baseurl, "file", model, lens, themes, null, "1_15", 1);
            GalleryFrame.Navigate(typeof(Pages.Gallary_Page), ReqUrl);
            await Task.Delay(100);
        }

    }
}
