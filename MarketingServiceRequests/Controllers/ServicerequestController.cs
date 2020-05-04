using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingServiceRequests.Controllers
{
    public class ServicerequestController : Controller
    {
        // GET: Servicerequest
        //[HttpGet]
        //public ActionResult request()
        //{
        //    if (Session["Id"] == null)
        //        return RedirectToAction("Index", "User");

        //    return View();
        //}

        //[HttpPost]
        //[ActionName("request")]
        //public ActionResult request_post()
        //{
        //    if (ModelState != null && ModelState.IsValid) {
        //        return RedirectToAction("requestconfirmation", "Servicerequest");
        //    }
        //    return View();
        //}


        [HttpGet]
        public ActionResult requestconfirmation()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
           // return RedirectToAction("Index", "User");

            return View();
        }

        //[HttpPost]
        //[ActionName("requestconfirmation")]
        //public ActionResult request_post_post()
        //{
        //    if (ModelState != null && ModelState.IsValid)
        //    {
        //        return RedirectToAction("", "");
        //    }
        //    return View();
        //}
    }
}