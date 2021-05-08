using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AffinidiHackathon.Controllers
{
    public class HolderController : Controller
    {
        // GET: Holder
        public ActionResult Index()
        {
            return View();
        }

        // GET: Holder/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Holder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Holder/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Holder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Holder/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Holder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Holder/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
