using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace X.Web.Util
{
    public class IpBase
    {
        public static string GetIp(HttpContextWrapper context)
        {
            if (Equals(context, null)) return string.Empty;
            var ips = new[] { "127.0.0.1", context.Request.ServerVariables["REMOTE_ADDR"], context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] };
            var length = ips.Length;
            var ret = ips[0];
            while (length-- > 0)
            {
                if (string.IsNullOrWhiteSpace(ips[length]) || "::1".Equals(ips[length])) continue;
                ret = ips[length];
                break;
            }
            return ret;
        }

        public static string GetIp()
        {
            return GetIp(new HttpContextWrapper(HttpContext.Current));
        }

        public static string GetLocalIp()
        {
            var ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ip = "127.0.0.1";
            foreach (var ipa in ipEntry.AddressList)
            {
                ip = ipa.ToString();
                if (!ipa.IsIPv6LinkLocal && !ipa.IsIPv6Multicast && !ipa.IsIPv6SiteLocal && !ipa.IsIPv6Teredo &&
                    !ip.StartsWith("172.") && !ip.StartsWith("192.") && !ip.StartsWith("168.") &&
                    !ip.Contains("61.129.84.142") && !ip.Contains("140.207.218.45") && IpValid(ip)) break;
            }
            return ip;
        }

        public static bool IpValid(string ip)
        {
            return Regex.IsMatch(ip, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        }
    }
}
