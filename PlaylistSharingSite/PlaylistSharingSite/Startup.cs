using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PlaylistSharingSite.Startup))]
namespace PlaylistSharingSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
