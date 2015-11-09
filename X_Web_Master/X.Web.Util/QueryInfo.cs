using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace X.Web.Util
{
    public class QueryInfo
    {
        #region GetQuery
        public static string GetQueryString(HttpContextWrapper httpContext, string strName, string defValue)
        {
            var result = defValue;
            try
            {
                var urlDecode = HttpUtility.UrlDecode(httpContext.Request.QueryString[strName] ?? defValue);
                if (urlDecode != null)
                    result = urlDecode.Trim();
            }
            catch
            {
                // ignored
            }
            return result;
        }

        public static bool GetQueryBoolean(HttpContextWrapper httpContext, string strName, bool defValue)
        {
            return ParseUtil.GetBoolean(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static byte GetQueryByte(HttpContextWrapper httpContext, string strName, byte defValue)
        {
            return ParseUtil.GetByte(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static short GetQueryShort(HttpContextWrapper httpContext, string strName, short defValue)
        {
            return ParseUtil.GetInt16(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static int GetQueryInt(HttpContextWrapper httpContext, string strName, int defValue)
        {
            return ParseUtil.GetInt32(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static long GetQueryLong(HttpContextWrapper httpContext, string strName, long defValue)
        {
            return ParseUtil.GetInt64(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static decimal GetQueryDecimal(HttpContextWrapper httpContext, string strName, decimal defValue)
        {
            return ParseUtil.GetDecimal(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static double GetQueryDouble(HttpContextWrapper httpContext, string strName, double defValue)
        {
            return ParseUtil.GetDouble(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static float GetQuerySingle(HttpContextWrapper httpContext, string strName, float defValue)
        {
            return ParseUtil.GetSingle(GetQueryString(httpContext, strName, string.Empty), defValue);
        }

        public static DateTime GetQueryDateTime(HttpContextWrapper httpContext, string strName, string format, DateTime defValue)
        {
            return ParseUtil.GetDateTime(GetQueryString(httpContext, strName, string.Empty), format, defValue);
        }
        #endregion

        #region PostQuery
        public static string GetPostString(HttpContextWrapper httpContext, string strName, string defValue)
        {
            if (Equals(httpContext, null)) return defValue;
            var dictionary = (IDictionary<string, object>)httpContext.Items["paramsDictionary"];
            if (dictionary != null) return dictionary.ContainsKey(strName) ? dictionary[strName].ToString() : defValue;
            var collection = (NameValueCollection)httpContext.Items["paramsCollection"];
            if (collection != null) return collection[strName] ?? defValue;
            var reader = new StreamReader(httpContext.Request.InputStream);
            var urlDecode = HttpUtility.UrlDecode(reader.ReadToEnd());
            if (Equals(urlDecode, null)) return string.Empty;
            var paramsInput = urlDecode.Trim();
            if (string.IsNullOrWhiteSpace(paramsInput)) return defValue;
            if (paramsInput.IsJson())
            {
                dictionary = paramsInput.FromJson<IDictionary<string, object>>();
                dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
                httpContext.Items["paramsDictionary"] = dictionary;
                return dictionary.ContainsKey(strName) ? dictionary[strName].ToString() : defValue;
            }
            collection = HttpUtility.ParseQueryString(paramsInput);
            httpContext.Items["paramsCollection"] = collection;
            return collection[strName] ?? defValue;
        }

        public static bool GetPostBoolean(HttpContextWrapper httpContext, string strName, bool defValue)
        {
            return ParseUtil.GetBoolean(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static byte GetPostByte(HttpContextWrapper httpContext, string strName, byte defValue)
        {
            return ParseUtil.GetByte(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static short GetPostShort(HttpContextWrapper httpContext, string strName, short defValue)
        {
            return ParseUtil.GetInt16(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static int GetPostInt(HttpContextWrapper httpContext, string strName, int defValue)
        {
            return ParseUtil.GetInt32(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static long GetPostLong(HttpContextWrapper httpContext, string strName, long defValue)
        {
            return ParseUtil.GetInt64(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static decimal GetPostDecimal(HttpContextWrapper httpContext, string strName, decimal defValue)
        {
            return ParseUtil.GetDecimal(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static double GetPostDouble(HttpContextWrapper httpContext, string strName, double defValue)
        {
            return ParseUtil.GetDouble(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static float GetPostSingle(HttpContextWrapper httpContext, string strName, float defValue)
        {
            return ParseUtil.GetSingle(GetPostString(httpContext, strName, string.Empty), defValue);
        }

        public static DateTime GetPostDateTime(HttpContextWrapper httpContext, string strName, string format, DateTime defValue)
        {
            return ParseUtil.GetDateTime(GetPostString(httpContext, strName, string.Empty), format, defValue);
        }
        #endregion
    }
}
