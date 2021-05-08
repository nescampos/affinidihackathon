using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class BuildSignedVC
    {
        public SignedCredential signedCredential { get; set; }
    }

    public class SignedCredential
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

    public class Proof
    {
        public string type { get; set; }
        public DateTime created { get; set; }
        public string verificationMethod { get; set; }
        public string proofPurpose { get; set; }
        public string jws { get; set; }
    }
}