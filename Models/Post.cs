using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Anspeamiaincercareplusunu.Models
{
    public class Post
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Nu se pot da raspunsuri fara text.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        public DateTime Created { get; set; }
        //[Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; }

        public virtual IEnumerable<SelectListItem> Form { get; set; }
        public ICollection<PostReply> PostReplies { get; internal set; }
    }
}