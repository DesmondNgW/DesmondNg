using System.Collections.Generic;
using System.Text;
using X.Interface.Dto;

namespace X.Web.Util
{
    public class ApiData
    {
        /// <summary>
        /// GetUri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static string GetUri(string uri, Dictionary<string, string> arguments)
        {
            if (Equals(arguments, null) || arguments.Count <= 0) return uri;
            var sb = new StringBuilder();
            sb.Append(uri.Contains("?") ? uri : uri + "?");
            foreach (var arg in arguments) sb.Append($"&{arg.Key}={arg.Value}");
            return sb.ToString().Replace("?&", "?");
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="arguments"></param>
        /// <param name="extendHeaders"></param>
        /// <returns></returns>
        public static T Get<T>(string uri, Dictionary<string, string> arguments, Dictionary<string, string> extendHeaders)
        {
            var cookie = string.Empty;
            return HttpRequestBase.GetHttpInfo(GetUri(uri, arguments), "utf-8", "application/json", extendHeaders, ref cookie).FromJson<T>();
        }

        /// <summary>
        /// Post data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postdata"></param>
        /// <param name="extendHeaders"></param>
        /// <returns></returns>
        public static T Post<T>(string uri, object postdata, Dictionary<string, string> extendHeaders)
        {
            var cookie = string.Empty;
            return HttpRequestBase.PostHttpInfo(uri, "utf-8", postdata.ToJson(), "application/json", extendHeaders, ref cookie).FromJson<T>();
        }

        /// <summary>
        /// CallSuccess
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="iresult"></param>
        /// <returns></returns>
        public static bool CallSuccess<TResult>(ApiResult<TResult> iresult)
        {
            return iresult != null && iresult.Data != null && iresult.Success;
        }
    }
}
