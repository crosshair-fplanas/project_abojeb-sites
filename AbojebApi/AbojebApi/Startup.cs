using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AbojebApi.Startup))]
namespace AbojebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
