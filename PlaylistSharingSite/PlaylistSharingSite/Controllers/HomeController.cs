using System.Linq;
using System.Web.Mvc;
using PlaylistSharingSite.Models;

namespace PlaylistSharingSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index
        public ActionResult Index()
        {
            return RedirectToAction("List", "Playlist");
        }

        [HttpGet]
        public JsonResult GetPlayerData(int? playlistId)
        {
            
            using (var db = new PlaylistSharingDbContext())
            {
                var song = new AudioFile();
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);
                if (playlist != null)
                {



                    song = playlist.AudioFiles.FirstOrDefault();
                }
                var path = "http://" + Request.Url.Authority +"/Users/"+ playlist.UserId +"/" + song.NameOnServer;

                return Json(new { title = song.Title, url = path, duration = song.Duration }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult PlaySong(int? playlistId, int? songId)
        {
            if (playlistId != null && songId != null)
            {
                using (var db = new PlaylistSharingDbContext())
                {
                    var song = new AudioFile();
                    var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);
                    if (playlist != null)
                    {
                        song = playlist.AudioFiles.FirstOrDefault(s => s.Id == songId);
                    }
                    var path = "http://" + Request.Url.Authority + "/Users/" + playlist.UserId + "/" + song.NameOnServer;

                    return Json(new { title = song.Title, url = path, duration = song.Duration }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(Response);
        }
    }
}