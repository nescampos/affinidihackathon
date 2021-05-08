using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class BuildUnsignedVCRequest
    {
        public string type { get; set; }
        public string holderDid { get; set; }

        public CreateCredentialFormModel data { get; set; }
    }

}