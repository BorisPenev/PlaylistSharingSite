using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PlaylistSharingSite.Models;

namespace PlaylistSharingSite.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: File/UploadSongs
        [HttpGet]
        public ActionResult UploadSongs(int? playlistId)
        {
            
            using (var db = new PlaylistSharingDbContext())
            {
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);
                

                return View(playlist);
            }
        }

        // POST: File/UploadSongs
        [HttpPost]
        [ActionName("UploadSongs")]
        public ActionResult UploadSongsConfirmed(Playlist model, IEnumerable<HttpPostedFileBase> uploadSongs)
        {
            if (model == null || uploadSongs == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                // Get server folder or create one for the current user
                var folder = Directory.CreateDirectory(Server.MapPath("~/Users"));
                folder.CreateSubdirectory(this.User.Identity.GetUserId());

                using (PlaylistSharingDbContext db = new PlaylistSharingDbContext())
                {

                    foreach (var file in uploadSongs)
                    {
                        if (file.ContentLength > 0)
                        {
                            // Upload to server folder /Users/{user.Id}/fileName
                            var fileName = Path.GetFileName(file.FileName);
                            var serverPath = Path.Combine(Server.MapPath("~/Users"), this.User.Identity.GetUserId(), fileName);
                            file.SaveAs(serverPath);

                            TagLib.File fileDetails = TagLib.File.Create(serverPath);
                            // populate a song file

                            var song = new AudioFile(fileName, serverPath, fileDetails.Tag, fileDetails.Properties);

                            // Add songs to playlist
                            var playlist = db.Playlists.FirstOrDefault(p => p.Id == model.Id);
                            if (playlist != null)
                                playlist.AudioFiles.Add((song));
                        }
                    }

                    // Save to Database
                    db.SaveChanges();

                    return RedirectToAction("List", "Playlist");
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult DeleteSong(int? playlistId, int? songId)
        {
            if (playlistId == null || songId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new PlaylistSharingDbContext())
            {
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);

                // Check if playlist exists
                if (playlist == null)
                {
                    return HttpNotFound();
                }

                var song = playlist.AudioFiles.FirstOrDefault(a => a.Id == songId);
                
                // Check if song exists
                if (song == null)
                {
                    return HttpNotFound();
                }

                if (IsUserAuthorizedToEdit(playlist))
                {
                    var serverPath = Path.Combine(Server.MapPath("~/Users"), playlist.UserId, song.NameOnServer);
                    if (System.IO.File.Exists(serverPath))
                    {
                        System.IO.File.Delete(serverPath);
                    }

                    playlist.AudioFiles.Remove(song);
                }

                // Save
                db.SaveChanges();

                return RedirectToAction("Details", "Playlist", new {id = playlistId});
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