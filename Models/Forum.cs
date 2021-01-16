using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anspeamiaincercareplusunu.Models
{
    public class Forum
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu fha")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Post> Posts { get; internal set; }
    }
}