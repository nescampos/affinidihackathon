using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class CredentialVerified
    {
        public List<string> errors { get; set; }
        public bool isValid { get; set; }
    }
}