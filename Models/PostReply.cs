using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anspeamiaincercareplusunu.Models
{
    public class PostReply
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nu mai da comentarii goale pentru ca Sufar Sufar Sufar"), MinLength(3, ErrorMessage = "Nu scrii cu emojiuri ca sa ai doar 3 caractere.")]
        public string Content { get; set; }
        public DateTime Created { get; set; }

        
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}