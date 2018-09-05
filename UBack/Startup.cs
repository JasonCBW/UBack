using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UBack.Startup))]
namespace UBack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
