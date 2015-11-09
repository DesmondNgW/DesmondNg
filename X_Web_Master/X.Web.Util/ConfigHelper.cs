using System;
using System.Configuration;
using System.IO;
using System.Web.Caching;

namespace X.Web.Util
{
    public class ConfigHelper
    {
        private const string CacheConfigurationPrefix = "X.Web.Util.CacheConfigurationPrefix";
        public const int MaxPoolSize = 1;
        public static string ConfigurationFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Configs.xml");

        public static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? string.Empty;
        }

        public static string GetAppSetting(string name, string defaultValue)
        {
            var result = GetAppSetting(name);
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        #region GetAppSetting From Xml
        public static string GetAppSettingByName(string name, string defaultValue)
        {
            var key = CacheConfigurationPrefix + name + "GetAppSettingByName";
            var result = RunTimeCache.Get<string>(key);
            if (!string.IsNullOrWhiteSpace(result)) return result;
            result = defaultValue;
            var doc = XmlHelper.GetXmlDocCache(ConfigurationFile);
            if (Equals(doc, null)) return result;
            var node = doc.SelectSingleNode("/configuration/AppSettings/add[@key='" + name + "']");
            result = XmlHelper.GetXmlAttributeValue(node, "value", defaultValue);
            RunTimeCache.SlidingExpirationSet(key, result, new CacheDependency(ConfigurationFile, DateTime.Now), new TimeSpan(1, 0, 0), CacheItemPriority.AboveNormal);
            return result;
        }

        public static void UpdateAppSettingByName(string name, string value)
        {
            var doc = XmlHelper.GetXmlDocCache(ConfigurationFile);
            if (Equals(doc, null)) return;
            var node = doc.SelectSingleNode("/configuration/AppSettings/add[@key='" + name + "']");
            if (node?.Attributes != null)
            {
                var attr = node.Attributes["value"] ?? doc.CreateAttribute("value");
                attr.Value = value;
                node.Attributes.SetNamedItem(attr);
            }
            doc.Save(ConfigurationFile);
        }

        public static byte GetAppSettingByName(string name, byte defaultValue)
        {
            return ParseUtil.GetByte(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static bool GetAppSettingByName(string name, bool defaultValue)
        {
            return ParseUtil.GetBoolean(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static short GetAppSettingByName(string name, short defaultValue)
        {
            return ParseUtil.GetInt16(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static int GetAppSettingByName(string name, int defaultValue)
        {
            return ParseUtil.GetInt32(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static long GetAppSettingByName(string name, long defaultValue)
        {
            return ParseUtil.GetInt64(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static float GetAppSettingByName(string name, float defaultValue)
        {
            return ParseUtil.GetSingle(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static double GetAppSettingByName(string name, double defaultValue)
        {
            return ParseUtil.GetDouble(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static decimal GetAppSettingByName(string name, decimal defaultValue)
        {
            return ParseUtil.GetDecimal(GetAppSettingByName(name, string.Empty), defaultValue);
        }

        public static DateTime GetAppSettingByName(string name, DateTime defaultValue)
        {
            return ParseUtil.GetDateTime(GetAppSettingByName(name, string.Empty), defaultValue);
        }
        #endregion
    }
}
