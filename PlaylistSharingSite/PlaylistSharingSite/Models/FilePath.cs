using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PlaylistSharingSite.Models
{
    public class FilePath
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public string FileType { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}