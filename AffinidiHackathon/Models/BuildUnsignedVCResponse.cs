using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class BuildUnsignedVCResponse
    {
        [JsonProperty("unsignedVC")]
        public UnsignedVC unsignedCredential { get; set; }
    }

    public class UnsignedVC
    {
        [JsonProperty("@context")]
        public List<object> Context { get; set; }
        public string id { get; set; }
        public List<string> type { get; set; }
        public Holder holder { get; set; }
        public CredentialSubject credentialSubject { get; set; }
        public DateTime issuanceDate { get; set; }
    }

    public class CredentialSubject
    {
        public Data data { get; set; }
    }

    public class Holder
    {
        public string id { get; set; }
    }

    public class Data
    {
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Passport { get; set; }
        public string PassportNumber { get; set; }
        public string NationalDocument { get; set; }
        public string NationalDocumentNumber { get; set; }
        public string Photo { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
    }
}