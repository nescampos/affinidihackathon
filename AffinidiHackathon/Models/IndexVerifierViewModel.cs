using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class IndexVerifierViewModel
    {
        public string Url { get; set; }
        public CredentialShared Credential { get; set; }
        public CredentialVerified Verified { get; set; }
    }
}