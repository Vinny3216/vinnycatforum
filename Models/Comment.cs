using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vinnycatforum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

     
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public int DiscussionId { get; set; }

        public Discussion? Discussion { get; set; }  // nullable!!
    }
}
