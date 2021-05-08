using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class IndexAdminViewModel
    {
        public Dictionary<string, List<BuildUnsignedVCResponse>> Pending { get; set; }
    }
}