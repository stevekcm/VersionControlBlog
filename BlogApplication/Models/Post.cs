using BlogApplication.Models.Comment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    
    public class Post 
    {


        public int PostId { get; set; }
        public string Title { get; set; }
        public string Categories { get; set; }
        public string Introduction { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Author { get; set; }
        //public int View { get; set; }
        //public int Liked { get; set; }
       
        
        
        public List<MainComment> MainComments { get; set; }

   
    }
}
