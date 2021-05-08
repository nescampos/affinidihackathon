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
    public class AdminController : BaseController
    {
        public ActionResult Login()
        {
            if (Session["Admin"] != null)
            {
                return RedirectToAction("Index");
            }
            AdminSignupViewModel model = new AdminSignupViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(AdminSignUp Form)
        {

            AdminSignupViewModel model = new AdminSignupViewModel();
            model.Form = Form;
            var json = JsonConvert.SerializeObject(new { username = Form.username, password = Form.password });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = URLAffinidy + "/users/login";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                var response = client.PostAsync(url, data).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var respuesta = JsonConvert.DeserializeObject<ResponseLogin>(result);
                if (respuesta.accessToken != null)
                {
                    AccessToken = respuesta.accessToken;
                    Admin = Form.username;
                    HolderDid = respuesta.did;
                    if (!UserDids.ContainsKey(Form.username))
                    {
                        UserDids.Add(Form.username, respuesta.did);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = respuesta.message;
                    return View(model);
                }
            }

        }

        public ActionResult Logoff()
        {
            if(Session["Username"] != null)
            {
                UserDids.Remove(Username);
            }
            
            AccessToken = null;
            Admin = null;
            Username = null;
            HolderDid = null;
            return RedirectToAction("Index");
        }

        public ActionResult SignUp()
        {
            AdminSignupViewModel model = new AdminSignupViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(AdminSignUp Form)
        {
            AdminSignupViewModel model = new AdminSignupViewModel();
            model.Form = Form;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var json = JsonConvert.SerializeObject(new { username = Form.username, password=Form.password });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = URLAffinidy + "/users/signup";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                var response = client.PostAsync(url, data).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var respuesta = JsonConvert.DeserializeObject<ResponseSignUp>(result);
                if (respuesta.accessToken != null)
                {
                    UserDids.Add(Form.username, respuesta.did);
                    AdminCountry.Add(Form.username, Form.country);
                    ViewBag.Message = "Successful signup.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Error = respuesta.message;
                    return View(model);
                }
            }

        }

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }
            IndexAdminViewModel model = new IndexAdminViewModel();
            model.Pending = new Dictionary<string, List<BuildUnsignedVCResponse>>();
            if (AdminCountry.ContainsKey(Admin))
            {
                foreach(var cred in UnsignedCredentials)
                {
                    foreach(var valueCred in cred.Value)
                    {
                        if (valueCred.unsignedCredential.credentialSubject.data.Country == AdminCountry[Admin])
                        {
                            if (model.Pending.ContainsKey(cred.Key))
                            {
                                model.Pending[cred.Key].Add(valueCred);
                            }
                            else
                            {
                                model.Pending.Add(cred.Key, new List<BuildUnsignedVCResponse>() { valueCred });
                            }
                        }
                    }
                }
            }
            else
            {
                model.Pending = UnsignedCredentials;
            }
            return View(model);
        }

        public ActionResult ViewCredential(string user)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewCredentialAdminViewModel model = new ViewCredentialAdminViewModel();
            model.Credentials = UnsignedCredentials[user];
            model.User = user;
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult ViewDetails(string user, string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewDetailsAdminViewModel model = new ViewDetailsAdminViewModel();
            model.Credential = UnsignedCredentials[user].SingleOrDefault(x => x.unsignedCredential.id.Replace("claimId:", "") == id);
            model.User = user;
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SignCredential(string user, string id)
        {
            var credencial = UnsignedCredentials[user].SingleOrDefault(x => x.unsignedCredential.id.Replace("claimId:", "") == id);
            var json = JsonConvert.SerializeObject(credencial);
            json = json.Replace("unsignedVC", "unsignedCredential");
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = URLAffinidy + "/wallet/sign-credential";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                client.DefaultRequestHeaders.Add("Authorization", AccessToken);
                var response = client.PostAsync(url, data).Result;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var respuesta = JsonConvert.DeserializeObject<BuildSignedVC>(result);
                    if (SignedNotSavedCredentials.ContainsKey(user))
                    {
                        SignedNotSavedCredentials[user].Add(respuesta);
                    }
                    else
                    {
                        SignedNotSavedCredentials.Add(user, new List<BuildSignedVC>() { respuesta });
                    }
                    UnsignedCredentials[user].Remove(credencial);
                    return RedirectToAction("Index");
                }

                ViewBag.Error = response.StatusCode.ToString() + ":" + response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("ViewDetails");
            }
        }

        public ActionResult CredentialSigned()
        {
            return View();
        }
    }
}