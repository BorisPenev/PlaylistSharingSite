using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PlaylistSharingSite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.favoritePlaylists = new HashSet<Playlist>();
        }

        [Required]
        public string FullName { get; set; }

        private ICollection<Playlist> favoritePlaylists;
        public virtual ICollection<Playlist> FavoritePlaylists
        {
            get { return favoritePlaylists; }
            set { favoritePlaylists = value; }
        }

        public virtual ICollection<FilePath> ProfilePicturePaths { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}