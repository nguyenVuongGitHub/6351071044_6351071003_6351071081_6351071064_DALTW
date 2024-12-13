using Dookki_Web.App_Start;
using Dookki_Web.Models;
using Dookki_Web.Models.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Dookki_Web.Contents
{
    public class HomeController : Controller
    {
        DOOKKIEntities db = new DOOKKIEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Order()
        {
            return View(db.Tickets.ToList());
        }
        public ActionResult Search(string keyword)
        {
            var result = db.Tickets
                           .Where(t => t.Name.Contains(keyword))
                           .ToList();
            return View("Order", result);
        }
    }
}