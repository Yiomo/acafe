using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace αcafe.Functions
{
    class Setting
    {
        public static bool IsFirstlyRun()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["BackgroundSource"] == null)
                return true;
            else
                return false;
        }

        public static bool InitializeSettings()
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["BackgroundSource"] = "Bing";
                localSettings.Values["DownloadPath"] = "我的文档\\图片\\Acafe";
                localSettings.Values["DownloadQuality"] = "Raw";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetSettingValue(string key,string value)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[key ] = value;
            return true;
        }

        public static string GetSettingValue(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string result  = localSettings.Values[key] as string;
            return result;
        }
    }
}
