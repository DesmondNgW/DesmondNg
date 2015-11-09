using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

namespace X.Web.Util
{
    public class XmlHelper
    {
        #region Load && Write && Read Xml
        private const string XmlPrefix = "X.Web.Util.XmlPrefix";
        private static XmlDocument GetXmlDoc(string filePath)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(filePath);
            }
            catch (Exception ex)
            {
                Logger.Error(MethodBase.GetCurrentMethod(), LogDomain.Util, null, "Xml error:" + ex);
            }
            return doc;
        }

        public static XmlDocument GetXmlDocCache(string filePath)
        {
            var key = XmlPrefix + filePath;
            var cache = RunTimeCache.Get<XmlDocument>(key);
            if (cache != null) return cache;
            lock (FileBase.Getlocker(key))
            {
                cache = RunTimeCache.Get<XmlDocument>(key);
                if (cache != null) return cache;
                cache = GetXmlDoc(filePath);
                RunTimeCache.SlidingExpirationSet(key, cache, new CacheDependency(filePath, DateTime.Now), new TimeSpan(1, 0, 0), CacheItemPriority.Normal);
            }
            return cache;
        }

        public static void WriteXml<T>(T item, string path)
        {
            FileBase.CoderLocker(path, () =>
            {
                var serializer = new XmlSerializer(typeof(T));
                try
                {
                    var xw = XmlWriter.Create(path);
                    serializer.Serialize(xw, item);
                    xw.Close();
                }
                catch (Exception ex)
                {
                    Logger.Error(MethodBase.GetCurrentMethod(), LogDomain.Util, null, "WriteXml error:" + ex);
                }
            });
        }

        public static XmlDocument GetXmlDocument(object item)
        {
            var xml = new XmlDocument();
            var serializer = new XmlSerializer(item.GetType());
            var sw = new StringWriter(CultureInfo.InvariantCulture);
            serializer.Serialize(sw, item);
            xml.LoadXml(sw.ToString());
            sw.Close();
            return xml;
        }

        public static T ReadXmlCache<T>(string path)
        {
            if (!File.Exists(path)) return default(T);
            var key = XmlPrefix + path + typeof(T).FullName;
            var cache = RunTimeCache.Get<T>(key);
            if (cache != null) return cache;
            lock (FileBase.Getlocker(key))
            {
                cache = RunTimeCache.Get<T>(key);
                if (cache != null) return cache;
                var serializer = new XmlSerializer(typeof(T));
                var xr = XmlReader.Create(path, null);
                cache = (T)serializer.Deserialize(xr);
                RunTimeCache.SlidingExpirationSet(key, cache, new CacheDependency(path, DateTime.Now), new TimeSpan(1, 0, 0), CacheItemPriority.Normal);
            }
            return cache;
        }
        #endregion

        #region String
        public static string GetXmlNodeValue(XmlNode node, string defaultValue)
        {
            var ret = defaultValue;
            if (!string.IsNullOrWhiteSpace(node?.InnerText.Trim())) ret = node.InnerText.Trim();
            return ret;
        }

        public static string GetXmlNodeXml(XmlNode node, string defaultValue)
        {
            var ret = defaultValue;
            if (!string.IsNullOrWhiteSpace(node?.InnerXml.Trim())) ret = node.InnerXml.Trim();
            return ret;
        }

        public static string GetXmlAttributeValue(XmlNode node, string attributeName, string defaultValue)
        {
            var ret = defaultValue;
            if (Equals(node, null)) return ret;
            var attribute = node.Attributes?[attributeName];
            if (!string.IsNullOrWhiteSpace(attribute?.Value.Trim())) ret = attribute.Value.Trim();
            return ret;
        }
        #endregion

        #region Boolean
        public static bool GetXmlNodeValue(XmlNode node, bool defaultValue)
        {
            return ParseUtil.GetBoolean(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static bool GetXmlAttributeValue(XmlNode node, string attributeName, bool defaultValue)
        {
            return ParseUtil.GetBoolean(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Byte
        public static byte GetXmlNodeValue(XmlNode node, byte defaultValue)
        {
            return ParseUtil.GetByte(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static byte GetXmlAttributeValue(XmlNode node, string attributeName, byte defaultValue)
        {
            return ParseUtil.GetByte(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Int16
        public static short GetXmlNodeValue(XmlNode node, short defaultValue)
        {
            return ParseUtil.GetInt16(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static short GetXmlAttributeValue(XmlNode node, string attributeName, short defaultValue)
        {
            return ParseUtil.GetInt16(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Int32
        public static int GetXmlNodeValue(XmlNode node, int defaultValue)
        {
            return ParseUtil.GetInt32(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static int GetXmlAttributeValue(XmlNode node, string attributeName, int defaultValue)
        {
            return ParseUtil.GetInt32(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Int64
        public static long GetXmlNodeValue(XmlNode node, long defaultValue)
        {
            return ParseUtil.GetInt64(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static long GetXmlAttributeValue(XmlNode node, string attributeName, long defaultValue)
        {
            return ParseUtil.GetInt64(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Single
        public static float GetXmlNodeValue(XmlNode node, float defaultValue)
        {
            return ParseUtil.GetSingle(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static float GetXmlAttributeValue(XmlNode node, string attributeName, float defaultValue)
        {
            return ParseUtil.GetSingle(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Double
        public static double GetXmlNodeValue(XmlNode node, double defaultValue)
        {
            return ParseUtil.GetDouble(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static double GetXmlAttributeValue(XmlNode node, string attributeName, double defaultValue)
        {
            return ParseUtil.GetDouble(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region Decimal
        public static decimal GetXmlNodeValue(XmlNode node, decimal defaultValue)
        {
            return ParseUtil.GetDecimal(GetXmlNodeValue(node, string.Empty), defaultValue);
        }

        public static decimal GetXmlAttributeValue(XmlNode node, string attributeName, decimal defaultValue)
        {
            return ParseUtil.GetDecimal(GetXmlAttributeValue(node, attributeName, string.Empty), defaultValue);
        }
        #endregion

        #region DateTime
        public static DateTime GetXmlNodeValue(XmlNode node, DateTime defaultValue, string format)
        {
            return ParseUtil.GetDateTime(GetXmlNodeValue(node, string.Empty), format, defaultValue);
        }

        public static DateTime GetXmlAttributeValue(XmlNode node, string attributeName, DateTime defaultValue, string format)
        {
            return ParseUtil.GetDateTime(GetXmlAttributeValue(node, attributeName, string.Empty), format, defaultValue);
        }
        #endregion
    }
}
