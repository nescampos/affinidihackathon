using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AffinidiHackathon.Controllers
{
    public class IssuerController : Controller
    {
        // GET: Issuer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignCredential()
        {
            return View();
        }
    }
}