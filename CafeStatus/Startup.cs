using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CafeStatus.Startup))]
namespace CafeStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
