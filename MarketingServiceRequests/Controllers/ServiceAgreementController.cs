using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingServiceRequests.Controllers
{
    public class ServiceAgreementController : Controller
    {
        // GET: ServiceAgreement
        [HttpGet]
        public ActionResult Agreement()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }
        
        [ActionName("AgreementSubmit")]
        public ActionResult Agreement_post()
        {            
           return RedirectToAction("requestconfirmation", "Servicerequest");         
        }

        public ActionResult RequestConfirmation()
        {
            Session.Remove("Id");
            //return RedirectToAction("Index", "User");

            //Sakthi Changes
            //return RedirectToAction("servicedetail", "Servicedetails");

            //Venkat Changes
            return RedirectToAction("HomeGrid", "Home");
        }
    }
}