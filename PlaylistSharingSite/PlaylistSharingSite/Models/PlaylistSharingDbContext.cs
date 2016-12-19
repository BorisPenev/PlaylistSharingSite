using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PlaylistSharingSite.Models
{    
    public class PlaylistSharingDbContext : IdentityDbContext<ApplicationUser>
    {
        public PlaylistSharingDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static PlaylistSharingDbContext Create()
        {
            return new PlaylistSharingDbContext();
        }
    }
}