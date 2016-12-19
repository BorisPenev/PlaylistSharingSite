
using MediaInfoDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlaylistSharingSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> LoadAudio()
        {
            TagLib.File file = TagLib.File.Create("E:\\downloads\\videoplayback.m4a");
            file.
            //var mediaFile = new MediaFile("E:\\downloads\\videoplayback.m4a");
            
            //var mi = new MediaInfo();
            //mi.Open(@"E:\downloads\videoplayback.m4a");
            //var audio = mediaFile.Audio;
            //var info = mediaFile.Inform;
            //var videoInfo = new VideoInfo(mi);
            // var audioInfo = new AudioInfo(mi);
            //mi.Close();

            return File(@"E:\downloads\videoplayback.m4a", "audio/mpeg");
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