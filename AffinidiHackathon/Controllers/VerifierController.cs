using AffinidiHackathon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AffinidiHackathon.Controllers
{
    public class VerifierController : BaseController
    {
        // GET: Verifier
        public ActionResult Index(string urlCredential)
        {
            IndexVerifierViewModel model = new IndexVerifierViewModel();
            if(!string.IsNullOrWhiteSpace(urlCredential))
            {
                model.Url = urlCredential;
                string resultadoCredenciales = string.Empty;

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(urlCredential).Result;

                    string result = response.Content.ReadAsStringAsync().Result;
                    var respuesta = JsonConvert.DeserializeObject<CredentialShared>(result);
                    model.Credential = respuesta;
                    resultadoCredenciales = result;
                }
                var json = JsonConvert.SerializeObject(new { verifiableCredentials = new List<CredentialShared> { model.Credential } });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = AffinidyURLVerifier + "/verifier/verify-vcs";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                    var response = client.PostAsync(url, data).Result;

                    string result = response.Content.ReadAsStringAsync().Result;
                    var respuesta = JsonConvert.DeserializeObject<CredentialVerified>(result);
                    model.Verified = respuesta;
                }
            }
            return View(model);
        }

    }
}