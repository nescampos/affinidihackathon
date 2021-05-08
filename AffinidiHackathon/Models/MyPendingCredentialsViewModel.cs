using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class MyPendingCredentialsViewModel
    {
        public List<BuildSignedVC> Credentials { get; set; }
        public string User { get; set; }
    }
}