using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class CredentialShared
    {
        [JsonProperty("@context")]
        public List<object> Context { get; set; }
        public string id { get; set; }
        public List<string> type { get; set; }
        public Holder holder { get; set; }
        public CredentialSubject credentialSubject { get; set; }
        public DateTime issuanceDate { get; set; }
        public string issuer { get; set; }
        public Proof proof { get; set; }
    }
}