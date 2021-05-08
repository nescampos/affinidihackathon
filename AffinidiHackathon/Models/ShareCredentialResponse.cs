using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class ShareCredentialResponse
    {
        public string qrCode { get; set; }
        public string sharingUrl { get; set; }
    }
}