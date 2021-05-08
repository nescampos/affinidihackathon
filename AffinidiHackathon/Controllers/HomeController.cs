using AffinidiHackathon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AffinidiHackathon.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("MyCredentials");
            }
            SignupViewModel model = new SignupViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(SignUp Form)
        {
            
            SignupViewModel model = new SignupViewModel();
            model.Form = Form;
            var json = JsonConvert.SerializeObject(Form);
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
                    Username = Form.username;
                    HolderDid = respuesta.did;
                    if(!UserDids.ContainsKey(Form.username))
                    {
                        UserDids.Add(Form.username, respuesta.did);
                    }
                    return RedirectToAction("MyCredentials");
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
            UserDids.Remove(Username);
            AccessToken = null;
            Username = null;
            Admin = null;
            HolderDid = null;
            return RedirectToAction("Index");
        }

        public ActionResult SignUp()
        {
            SignupViewModel model = new SignupViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SignUp(SignUp Form)
        {
            SignupViewModel model = new SignupViewModel();
            model.Form = Form;
            var json = JsonConvert.SerializeObject(Form);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = URLAffinidy+ "/users/signup";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                var response = client.PostAsync(url, data).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var respuesta = JsonConvert.DeserializeObject<ResponseSignUp>(result);
                if(respuesta.accessToken != null)
                {
                    UserDids.Add(Form.username, respuesta.did);
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

        public ActionResult MyCredentials()
       {
            if(Username == null)
            {
                return RedirectToAction("Login");
            }
            var url = URLAffinidy + "/wallet/credentials";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                client.DefaultRequestHeaders.Add("Authorization", AccessToken);
                var response = client.GetAsync(url).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var respuesta = JsonConvert.DeserializeObject<CredentialsUser[]>(result);
                return View(respuesta);
                
            }
        }

        public ActionResult MyPendingCredentials()
        {
            if (Username == null)
            {
                return RedirectToAction("Login");
            }
            MyPendingCredentialsViewModel model = new MyPendingCredentialsViewModel();

            model.Credentials = SignedNotSavedCredentials.ContainsKey(Username)? SignedNotSavedCredentials[Username] : new List<BuildSignedVC>();
            model.User = Username;
            return View(model);
        }

        [HttpPost]
        public ActionResult MyPendingCredentials(string id)
        {
            var credencial = SignedNotSavedCredentials[Username].SingleOrDefault(x => x.signedCredential.id.Replace("claimId:", "") == id);
            var json = JsonConvert.SerializeObject(new { data = new List<SignedCredential>() { credencial.signedCredential } });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = URLAffinidy + "/wallet/credentials";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                client.DefaultRequestHeaders.Add("Authorization", AccessToken);
                var response = client.PostAsync(url, data).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SignedNotSavedCredentials[Username].Remove(credencial);
                    return RedirectToAction("MyCredentials");
                }

                ViewBag.Error = response.StatusCode.ToString() + ":" + response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("MyPendingCredentials");
            }
            
        }


        public ActionResult Share(string id)
        {
            if (Username == null)
            {
                return RedirectToAction("Login");
            }
            ShareCredentialViewModel model = new ShareCredentialViewModel();
            var url = URLAffinidy + string.Format("/wallet/credentials/claimId:{0}/share",id);
            var json = JsonConvert.SerializeObject(new { ttl="0" });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                client.DefaultRequestHeaders.Add("Authorization", AccessToken);
                var response = client.PostAsync(url, data).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                model.Share = JsonConvert.DeserializeObject<ShareCredentialResponse>(result);

                return View(model);
            }
            
        }


        public ActionResult CreateCredential()
        {
            if (Username == null)
            {
                return RedirectToAction("Login");
            }
            CreateCredentialViewModel model = new CreateCredentialViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCredential(CreateCredentialFormModel Form)
        {
            CreateCredentialViewModel model = new CreateCredentialViewModel();
            model.Form = Form;
            Form.type = "NatIDNumCredentialPersonV1";
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }
            string user = UserDids.FirstOrDefault(x => x.Value == HolderDid).Key;

            BuildUnsignedVCRequest requestVC = new BuildUnsignedVCRequest { data = Form, holderDid = HolderDid, type = "NatIDNumCredentialPersonV1" };
            var json = JsonConvert.SerializeObject(requestVC);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = AffinidyURLIssuer + "/vc/build-unsigned";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                var response = client.PostAsync(url, data).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var respuesta = JsonConvert.DeserializeObject<BuildUnsignedVCResponse>(result);
                if(UnsignedCredentials.ContainsKey(user))
                {
                    UnsignedCredentials[user].Add(respuesta);
                }
                else
                {
                    UnsignedCredentials.Add(user, new List<BuildUnsignedVCResponse>() { respuesta });
                }
                
                ViewBag.Message = "Successful signup.";
                return RedirectToAction("CredentialCreated");
            }
        }

        public ActionResult CredentialCreated()
        {
            if (Username == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

    }
}