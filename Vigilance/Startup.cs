using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vigilance.Startup))]
namespace Vigilance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
