using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class CreateCredentialFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string BirthDate { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Passport { get; set; }
        [Required]
        public string PassportNumber { get; set; }
        [Required]
        public string NationalDocument { get; set; }
        [Required]
        public string NationalDocumentNumber { get; set; }
        [Required]
        public string Photo { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
    }
}