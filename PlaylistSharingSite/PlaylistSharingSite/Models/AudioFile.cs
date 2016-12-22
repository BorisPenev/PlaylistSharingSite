﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml;
using TagLib;

namespace PlaylistSharingSite.Models
{
    public class AudioFile
    {
        public AudioFile()
        {
            this.DateAdded = DateTime.Now;
        }

        public AudioFile(string nameOnServer, string filePath, TagLib.Tag tag, TagLib.Properties properties)
        {
            this.NameOnServer = new Guid().ToString();
            this.Title = tag.Title;

            //this.Path = string.Format("/AudioFiles/{0}", tag.Title);
            this.Path = filePath;
            this.Duration = properties.Duration;
            this.DateAdded = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string NameOnServer { get; set; }

        public string Path { get; set; }
        
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime DateAdded { get; set; }
    }
}