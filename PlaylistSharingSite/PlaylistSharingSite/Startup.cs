using System.Data.Entity;
using Microsoft.Owin;
using Owin;
using PlaylistSharingSite.Migrations;
using PlaylistSharingSite.Models;

[assembly: OwinStartupAttribute(typeof(PlaylistSharingSite.Startup))]
namespace PlaylistSharingSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PlaylistSharingDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
