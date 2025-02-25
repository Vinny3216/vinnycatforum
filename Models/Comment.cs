using System;
using System.ComponentModel.DataAnnotations;        // 必备：用于数据验证
using System.ComponentModel.DataAnnotations.Schema; // 可选：如果以后需要 [ForeignKey] 可以用到

namespace vinnycatforum.Models
{
    public class Comment
    {
        // 主键
        public int CommentId { get; set; }

        // 评论内容
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        // 图片上传
        [NotMapped] // 不映射到数据库
        public IFormFile? ImageFile { get; set; }  // 用于上传图片

        // 图片文件名
        public string ImageFilename { get; set; } = string.Empty;

        // 创建时间
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // 外键
        public int DiscussionId { get; set; }

        // 导航属性：多对一关系
        public Discussion? Discussion { get; set; }  // nullable!!
    }
}
