using System.Drawing;

namespace X.Web.Entities
{
    public class CaptchaModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Color[] Colors { get; set; }
        public string[] Fonts { get; set; }
        public string Characters { get; set; }
        public string Value { get; set; }
        public string CookieName { get; set; }
    }
}
