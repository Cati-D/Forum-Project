using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anspeamiaincercareplusunu.Models
{
    public class UserFriend
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string FriendId { get; set; }

        public ApplicationUser Friend { get; set; }

        public virtual ICollection<ApplicationUser> Friends { get; set; }
    }
}