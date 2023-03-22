using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TEMIS.CMS.Startup))]
namespace TEMIS.CMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
