using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PlaylistSharingSite.Models;
using PlaylistSharingSite.ViewModels;

namespace PlaylistSharingSite.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: User/List
        public ActionResult List()
        {
            using (var db = new PlaylistSharingDbContext())
            {
                var users = db.Users.ToList();

                var admins = GetAdminUserNames(users, db);
                ViewBag.Admins = admins;

                return View(users);
            }
        }
        // GET: User/Edit
        public ActionResult Edit(string id)
        {
            // Validate Id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new PlaylistSharingDbContext())
            {
                // Get user from database
                var user = db.Users.FirstOrDefault(u => u.Id == id);

                // Check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Create view model
                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, db);

                // Pass the model to the view
                return View(viewModel);
            }
        }

        // POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new PlaylistSharingDbContext())
                {
                    // Get user from database
                    var user = db.Users
                        .FirstOrDefault(a => a.Id == id);

                    // Check if user exists
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    // If password field is not empty, change password
                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var hasher = new PasswordHasher();
                        var passwordHash = hasher.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }
                    // Set user properties
                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles(viewModel, user, db);

                    // Save changes
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(viewModel);
        }

        // GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new PlaylistSharingDbContext())
            {
                // Get user from database
                var user = db.Users
                    .FirstOrDefault(u => u.Id.Equals(id));

                // Check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
        }

        // POST: User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new PlaylistSharingDbContext())
            {
                // Get user from database
                var user = db.Users
                   .FirstOrDefault(u => u.Id.Equals(id));

                // Get user playlists from database
                var userPlaylists = db.Playlists
                    .Where(u => u.User.Id.Equals(user.Id));

                // Delete user playlists
                foreach (var playlist in userPlaylists)
                {
                    db.Playlists.Remove(playlist);
                }

                // Get user ProfilePicturePaths from Database
                var userProfilePicturePaths = db.ProfilePicturePaths
                    .Where(u => u.User.Id.Equals(user.Id));

                // Delete user profile pictures
                foreach (var userProfilePicturePath in userProfilePicturePaths)
                {
                    db.ProfilePicturePaths.Remove(userProfilePicturePath);
                }

                // Delete User folder with all the files on Server
                var fullPath = Server.MapPath("~/Users/" + user.Id);
                var isUserDirectoryValid = Directory.Exists(fullPath);
                if (isUserDirectoryValid)
                {
                    Directory.Delete(fullPath, true);
                }

                // Delete user and save database
                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }
        private void SetUserRoles(EditUserViewModel viewModel, ApplicationUser user, PlaylistSharingDbContext context)
        {
            var userManager = HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in viewModel.Roles)
            {
                if (role.IsSelected && !userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected && userManager.IsInRole(user.Id, role.Name))
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        private List<Role> GetUserRoles(ApplicationUser user, PlaylistSharingDbContext db)
        {
            // Create user manager
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            // Get all application roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            // For each application role, check if the user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role() { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            // Return a list with all roles
            return userRoles;
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, PlaylistSharingDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    admins.Add(user.UserName);
                }
            }

            return admins;
        }
    }
}