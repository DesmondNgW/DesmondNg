using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(X.Web.MVC.Startup))]
namespace X.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
