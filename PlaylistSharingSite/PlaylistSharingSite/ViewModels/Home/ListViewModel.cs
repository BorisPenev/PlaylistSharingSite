using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlaylistSharingSite.Models;

namespace PlaylistSharingSite.ViewModels.Home
{
    public class ListViewModel
    {
        public string PlaylistTitle { get; set; }
        public ICollection<AudioFile> Songs { get; set; }
    }
}