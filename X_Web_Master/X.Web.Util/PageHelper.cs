using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using X.Web.Entities;

namespace X.Web.Util
{
    public class PageHelper
    {
        /// <summary>
        /// Url路径相关正则
        /// </summary>
        private static readonly Regex DotRe = new Regex(@"\/\.\/");
        private static readonly Regex DoubleDotRe = new Regex(@"\/[^/]+\/\.\.\/");
        private static readonly Regex MultiSlashRe = new Regex(@"([^:/])\/+\/");

        /// <summary>
        /// 解析成最终路径，去除多余的字符和相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRealPath(string path)
        {
            MatchEvaluator matchEvaluator = m => "/";
            path = DotRe.Replace(path, matchEvaluator);
            path = MultiSlashRe.Replace(path, m => m.Groups[1].ToString() + "/");
            while (DoubleDotRe.IsMatch(path)) path = DoubleDotRe.Replace(path, matchEvaluator);
            return path;
        }

        /// <summary>
        /// 验证码默认选项
        /// </summary>
        /// <returns></returns>
        private static CaptchaModel GetDefaultModel()
        {
            var options = new CaptchaModel
            {
                Width = 150,
                Height = 30,
                CookieName = "CAPTCHAModel",
                Colors = new[] { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkBlue },
                Characters = "23456789ABCDEFGHJKLMNPRSTWXY",
                Fonts = new[] { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" },
                Value = string.Empty
            };
            for (var i = 0; i < 6; i++)
            {
                options.Value += options.Characters[StringConvert.SysRandom.Next(options.Characters.Length)];
            }
            return options;
        }

        /// <summary>
        /// 1*1 像素图片
        /// </summary>
        /// <param name="context"></param>
        public static void ImageInfo(HttpContextWrapper context)
        {
            var bmp = new Bitmap(1, 1);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                context.Response.ClearContent();
                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                bmp.Dispose();
                g.Dispose();
                ms.Dispose();
            }
        }

        /// <summary>
        /// 验证码图片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        public static void Captcha(HttpContextWrapper context, CaptchaModel options)
        {
            if (Equals(options, null)) options = GetDefaultModel();
            int width = options.Width, height = options.Height;
            string value = options.Value, cookieName = options.CookieName;
            var colors = options.Colors;
            var fonts = options.Fonts;
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            for (var i = 0; i < 3; i++)
            {
                var x1 = StringConvert.SysRandom.Next(width);
                var y1 = StringConvert.SysRandom.Next(height);
                var x2 = StringConvert.SysRandom.Next(width);
                var y2 = StringConvert.SysRandom.Next(height);
                var clr = colors[StringConvert.SysRandom.Next(colors.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            for (var i = 0; i < value.Length; i++)
            {
                var fnt = fonts[StringConvert.SysRandom.Next(fonts.Length)];
                var ft = new Font(fnt, 16);
                var clr = colors[StringConvert.SysRandom.Next(colors.Length)];
                g.DrawString(value[i].ToString(), ft, new SolidBrush(clr), (float)i * 20 + 20, 6);
            }
            for (var i = 0; i < 50; i++)
            {
                var x = StringConvert.SysRandom.Next(bmp.Width);
                var y = StringConvert.SysRandom.Next(bmp.Height);
                var clr = colors[StringConvert.SysRandom.Next(colors.Length)];
                bmp.SetPixel(x, y, clr);
            }
            var ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                context.Response.ClearContent();
                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                bmp.Dispose();
                g.Dispose();
                ms.Dispose();
            }
            CookieHelper.SetCookie(context, cookieName, BaseCryption.SignData(cookieName, value, HmacType.Sha1));
        }

        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool VerifyCaptcha(HttpContextWrapper context, CaptchaModel options, string value)
        {
            if (Equals(options, null)) options = GetDefaultModel();
            return BaseCryption.VerifyData(options.CookieName, CookieHelper.GetCookie(context, options.CookieName), HmacType.Sha1);
        }
    }
}
