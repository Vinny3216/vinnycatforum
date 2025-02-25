using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;       // 必备：用于数据验证，例如 [Required]
using System.ComponentModel.DataAnnotations.Schema; // 必备：用于 [NotMapped] 等属性
using Microsoft.AspNetCore.Http;                   // 必备：用于 IFormFile 上传图片

namespace vinnycatforum.Models
{
    public class Discussion
    {
        // 主键
        public int DiscussionId { get; set; }

        // 标题
        [Required(ErrorMessage = "Title is required.")] // 数据验证
        public string Title { get; set; } = string.Empty;

        // 内容
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        // 图片文件名
        public string ImageFilename { get; set; } = string.Empty;

        // 上传图片
        [NotMapped] // 不映射到数据库
        public IFormFile? ImageFile { get; set; }  // 用于图片上传

        // 创建时间
        public DateTime CreateDate { get; set; } = DateTime.Now;

        // 导航属性：一对多关系
        public List<Comment>? Comments { get; set; }  // nullable!!
    }
}
