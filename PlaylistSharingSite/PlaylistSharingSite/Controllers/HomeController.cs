
using MediaInfoDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PlaylistSharingSite.Models;
using PlaylistSharingSite.ViewModels.Home;
using TagLib;

namespace PlaylistSharingSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/List
        public ActionResult List()
        {
            using (PlaylistSharingDbContext db = new PlaylistSharingDbContext())
            {
                if (db.Playlists.Count() == 0)
                {
                    var dirRelativePath = "/AudioFiles";
                    System.IO.DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + dirRelativePath);
                    var playlist = new Playlist();
                    playlist.Title = "Test Playlist Title";

                    foreach (var audioFile in dirInfo.GetFiles())
                    {
                        var fullPath = AppDomain.CurrentDomain.BaseDirectory + dirRelativePath + "/" + audioFile.Name;
                        TagLib.File file = TagLib.File.Create(fullPath);
                        
                        playlist.AudioFiles.Add(new AudioFile()
                        {
                            Path = dirRelativePath + "/" + audioFile.Name,
                            Duration = file.Properties.Duration,
                            NameOnServer = file.Name,
                            Title = audioFile.Name
                        });
                    }

                    db.Playlists.Add(playlist);
                    db.SaveChanges();
                }
                var playlists = db.Playlists.ToList();

                return View(playlists);
            }
        }

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
                return Json(new { title = song.Title, url = song.Path, duration = song.Duration }, JsonRequestBehavior.AllowGet);
            }


            //var file = TagLib.File.Create(AppDomain.CurrentDomain.BaseDirectory + filePath);
            //if (file != null)
            //{
            //    var model = new IndexViewModel
            //    {
            //        Duration = file.Properties.Duration,
            //        Url = "http://" + Request.Url.Authority + filePath
            //    };

            //    if (!file.Name.IsNullOrWhiteSpace())
            //        model.Title = file.Name;
            //    else
            //    {
            //        var fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + filePath);
            //        model.Title = fi.Name;
            //    }
            //}

            //RedirectToAction()
            //return Json(null, JsonRequestBehavior.AllowGet);
            //return View(model);
        }

        [HttpPost]
        public JsonResult GetPlayerDataConfirmed(int? playlistId)
        {
            //string uri = "E:\\downloads\\videoplayback.m4a";
            //TagLib.File file = null;
            //System.IO.FileInfo file_info = new System.IO.FileInfo(uri);

            //file = TagLib.File.Create(uri);
            //var duration = file.Properties.Duration;
            //AudioFile audioFile = new AudioFile();


            // Insert data into the audio file

            //return Json(audioFile.Path, audioFile.Title, JsonRequestBehavior.AllowGet);

            using (var db = new PlaylistSharingDbContext())
            {
                var song = new AudioFile();
                var playlist = db.Playlists.FirstOrDefault(p => p.Id == playlistId);
                if (playlist != null)
                {
                    song = playlist.AudioFiles.FirstOrDefault();
                }
                return Json(new { title = song.Title, url = song.Path, duration = song.Duration }, JsonRequestBehavior.AllowGet);
            }
            //return File(@"/AudioFiles/videoplayback.m4a", "audio/mpeg");
            //return File(@"F:\music\asd.mp3", "audio/mpeg");
        }
        
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}