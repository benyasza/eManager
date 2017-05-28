using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fodraszat.Startup))]
namespace Fodraszat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
