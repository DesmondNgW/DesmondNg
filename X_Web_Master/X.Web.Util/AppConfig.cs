using System;

namespace X.Web.Util
{
    public class AppConfig
    {
        /// <summary>
        /// cookie域
        /// </summary>
        public static string CookieDomain => ConfigHelper.GetAppSettingByName(nameof(CookieDomain), string.Empty);

        /// <summary>
        /// 时间戳
        /// </summary>
        public static string Timestamp => ConfigHelper.GetAppSettingByName(nameof(Timestamp), DateTime.Now.ToString("yyyyMMdd"));
    }
}
