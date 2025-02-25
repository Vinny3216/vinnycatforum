using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;       
using System.ComponentModel.DataAnnotations.Schema; 
using Microsoft.AspNetCore.Http;                   

namespace vinnycatforum.Models
{
    public class Discussion
    {
        
        public int DiscussionId { get; set; }

        
        [Required(ErrorMessage = "Title is required.")] 
        public string Title { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        
        public string ImageFilename { get; set; } = string.Empty;

        
        [NotMapped] 
        public IFormFile? ImageFile { get; set; }  

        
        public DateTime CreateDate { get; set; } = DateTime.Now;

        
        public List<Comment>? Comments { get; set; }  // nullable!!
    }
}
