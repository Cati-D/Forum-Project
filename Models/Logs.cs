﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anspeamiaincercareplusunu.Models
{
    public class Logs
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string log { get; set; }
    }
}