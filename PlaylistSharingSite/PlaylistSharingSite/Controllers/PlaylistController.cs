using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlaylistSharingSite.Models;

namespace PlaylistSharingSite.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult List()
        {
            List<Playlist> playlists;
            using (var database = new PlaylistSharingDbContext())
            {
                if (database.Playlists.Any())
                {
                    playlists = database.Playlists
                        .Include(u => u.User)
                        .Include(u => u.AudioFiles)
                        .ToList();
                }
                else
                {
                    playlists = new List<Playlist>();
                }
            }

            return View(playlists);
        }

        // GET: Playlist

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Playlist model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PlaylistSharingDbContext())
                {
                    // Get author id
                    var authorId = database.Users
                        .First(u => u.UserName == this.User.Identity.Name)
                        .Id;


                    var playlist = new Playlist(authorId, model.Title);


                    // Save article in DB
                    database.Playlists.Add(playlist);
                    database.SaveChanges();


                    return RedirectToAction("UploadSongs", "File", new {playlistId = playlist.Id});
                }

            }
            return View(model);
        }

        public ActionResult AddSongs(int? playlistId)
        {

            using (var db = new PlaylistSharingDbContext())
            {
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);


                return View(playlist);
            }
        }

        public ActionResult AddSongsConfirmed(Playlist model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new PlaylistSharingDbContext())
                {
                    // Get author id
                    var userId = database.Users
                        .First(u => u.UserName == this.User.Identity.Name)
                        .Id;


                    var playlist = new Playlist(userId, model.Title);


                    // Save article in DB
                    database.Playlists.Add(playlist);
                    database.SaveChanges();


                    return RedirectToAction("UploadSongs", "File", new {playlistId = playlist.Id});
                }
            }
            return View(model);
        }

        //GET:  Playlist/Details/id
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Playlist playlist;
            using (var database = new PlaylistSharingDbContext())
            {
                playlist = database.Playlists
                    .Where(a => a.Id == id)
                    .Include(a => a.User)
                    .Include(a => a.AudioFiles)
                    .FirstOrDefault();
            }

            return View(playlist);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new PlaylistSharingDbContext())
            {
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == id);

                // Check if playlist exists
                if (playlist == null)
                {
                    return HttpNotFound();
                }

               
                if (IsUserAuthorizedToEdit(playlist))
                {
                    foreach (var song in playlist.AudioFiles)
                    {
                        var serverPath = Path.Combine(Server.MapPath("~/Users"), playlist.UserId, song.NameOnServer);
                        if (System.IO.File.Exists(serverPath))
                        {
                            System.IO.File.Delete(serverPath);
                        }
                    }
                    playlist.AudioFiles.Clear();
                    db.Playlists.Remove(playlist);
                }

                // Save
                db.SaveChanges();

                return RedirectToAction("List", "Playlist");
            }
        }

        private bool IsUserAuthorizedToEdit(Playlist playlist)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = playlist.IsPlaylistAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}