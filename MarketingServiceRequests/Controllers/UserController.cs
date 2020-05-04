using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarketingServiceRequests.Models;

namespace MarketingServiceRequests.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserInformation paramuser)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                Utility objUtility = new Utility();
                Session["Id"] = objUtility.CreateRecord(paramuser, Session["ContactId"].ToString());
                return RedirectToAction("servicedetail", "Servicedetails");
            }
            return View();
        }        
    }
}