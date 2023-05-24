
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Inventaire_BackEnd.Startup))]

namespace Inventaire_BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
