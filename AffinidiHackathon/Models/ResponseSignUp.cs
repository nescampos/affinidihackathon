using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffinidiHackathon.Models
{
    public class ResponseSignUp
    {
        public string accessToken { get; set; }
        public string did { get; set; }

        public string serviceName { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public int? httpStatusCode { get; set; }
    }
}