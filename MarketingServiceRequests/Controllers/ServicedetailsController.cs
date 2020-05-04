using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MarketingServiceRequests.Models;

namespace MarketingServiceRequests.Controllers
{
    public class ServicedetailsController : Controller
    {
        // GET: Servicedetails
        [HttpGet]
        public ActionResult servicedetail()
        {
            //if (Session["Id"] == null)
            //    return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult servicedetail(ServiceDetails paramservicedetails, ServiceDetailsSelection paramcheckboxes)
        {
            if (Session["ContactId"] == null)
                return RedirectToAction("Login", "Login");

            if (ModelState != null && ModelState.IsValid)
            {
                Utility objUtility = new Utility();
                Session["Id"] = objUtility.CreateServiceDetail(paramservicedetails, paramcheckboxes, Session["ContactId"].ToString());
                //return RedirectToAction("servicecategories", "Servicedetails");
                return RedirectToAction("copywriting", "Services");

            }
            return View();
        }

        [HttpGet]
        [ActionName("servicecategories")]
        public ActionResult servicecategories()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        [ActionName("servicecategories")]
        public ActionResult servicecategories_post()
        {
            if (ModelState != null && ModelState.IsValid)
            {
                // return RedirectToAction("Agreement", "ServiceAgreement");
                return RedirectToAction("servicecategories", "Servicedetails");
            }
            return View();
        }





    }
}