using AffinidiHackathon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AffinidiHackathon.Controllers
{
    public class BaseController : Controller
    {

        protected string URLAffinidy = ConfigurationManager.AppSettings["AffinidyURL"];
        protected string HashAffinidy = ConfigurationManager.AppSettings["AffinidyAPIHash"];
        protected string AffinidyURLIssuer = ConfigurationManager.AppSettings["AffinidyURLIssuer"];
        protected string AffinidyURLMessage = ConfigurationManager.AppSettings["AffinidyURLMessage"];
        protected string AffinidyURLVerifier = ConfigurationManager.AppSettings["AffinidyURLVerifier"];
        public static Dictionary<string, string> UserDids = new Dictionary<string, string>();
        public static Dictionary<string, string> AdminCountry = new Dictionary<string, string>();
        public static Dictionary<string, List<BuildUnsignedVCResponse>> UnsignedCredentials = new Dictionary<string, List<BuildUnsignedVCResponse>>();
        public static Dictionary<string, List<BuildSignedVC>> SignedNotSavedCredentials = new Dictionary<string, List<BuildSignedVC>>();
        protected string Username
        {
            get
            {
                return Session["Username"].ToString();
            }
            set
            {
                Session["Username"] = value;
            }
        }

        protected string Admin
        {
            get
            {
                return Session["Admin"].ToString();
            }
            set
            {
                Session["Admin"] = value;
            }
        }

        protected string AccessToken
        {
            get
            {
                return Session["AccessToken"].ToString();
            }
            set
            {
                Session["AccessToken"] = value;
            }
        }

        protected string HolderDid
        {
            get
            {
                return Session["HolderDid"].ToString();
            }
            set
            {
                Session["HolderDid"] = value;
            }
        }
    }
}