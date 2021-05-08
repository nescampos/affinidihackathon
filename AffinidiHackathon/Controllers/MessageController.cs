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
    public class MessageController : BaseController
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send()
        {
            SendMessageViewModel model = new SendMessageViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Send(SendMessageFormModel Form)
        {
            SendMessageViewModel model = new SendMessageViewModel();
            model.Form = Form;
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var json = JsonConvert.SerializeObject(new { toDid=Form.User, message= new 
                { BirthDate = Form.BirthDate, City = Form.City, Country = Form.Country, Name = Form.Name, NationalDocument = Form.NationalDocument,
                NationalDocumentNumber = Form.NationalDocumentNumber,
                Passport = Form.Passport,
                PassportNumber = Form.PassportNumber,
                Photo = Form.Photo,
                Text = Form.Text}
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = AffinidyURLMessage + "/messages";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Key", HashAffinidy);
                client.DefaultRequestHeaders.Add("Authorization", AccessToken);
                var response = client.PostAsync(url, data).Result;

                ViewBag.Message = "Message sent successfully.";

                return RedirectToAction("Index");
            }

                
        }
    }
}