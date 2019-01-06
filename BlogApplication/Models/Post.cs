using BlogApplication.Models.Comment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    
    public class Post 
    {
        [Required]
        public int PostId { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string Title { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string Categories { get; set; }

        [Required, MinLength(2), MaxLength(500)]
        public string Introduction { get; set; }

        [Required, MinLength(2)]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string Author { get; set; }
        
        public List<MainComment> MainComments { get; set; }
  
    }
}
