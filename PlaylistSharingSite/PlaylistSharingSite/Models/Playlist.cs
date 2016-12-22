using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlaylistSharingSite.Models
{
    public class Playlist
    {
        public Playlist()
        {
            this.CreationDate = DateTime.Now;
            this.audioFiles = new HashSet<AudioFile>();
        }

        public Playlist(string userId, string title)
        {
            this.UserId = userId;
            this.Title = title;
            this.CreationDate = DateTime.Now;
            this.audioFiles = new HashSet<AudioFile>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        private ICollection<AudioFile> audioFiles;

        public virtual ICollection<AudioFile> AudioFiles
        {
            get { return audioFiles; }
            set { audioFiles = value; }
        }

        public bool IsPlaylistAuthor(string name)
        {
            return this.User.UserName.Equals(name);
        }
    }
}