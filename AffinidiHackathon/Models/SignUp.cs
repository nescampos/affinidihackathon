using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class SignUp
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}