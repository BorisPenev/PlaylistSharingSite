using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaylistSharingSite.ViewModels.Home
{
    public class IndexViewModel
    {
        public string Url { get; set; }  
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }
}