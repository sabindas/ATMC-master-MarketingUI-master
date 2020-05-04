using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarketingServiceRequests.Models;

namespace MarketingServiceRequests.Controllers
{
    public class ServicesController : Controller
    {
        Utility objUtility = new Utility();
        // GET: Services
        [HttpGet]
        public ActionResult copywriting()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult copywriting(ServiceCopyWriting paramservicecopywriting)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.CopyWriting(paramservicecopywriting, Session["Id"].ToString());
                return RedirectToAction("Agreement", "ServiceAgreement");

                //return RedirectToAction("Designs", "Services");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Designs()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult Designs(ServiceDesigns paramservicedesigns, HttpPostedFileBase Design_inputfile)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.Designs(paramservicedesigns, Session["Id"].ToString(), Design_inputfile);
                 return RedirectToAction("Agreement", "ServiceAgreement");
                //  return RedirectToAction("servicecategories", "Servicedetails");
               // return RedirectToAction("Events", "Services");

            }
            return View();
        }

        [HttpGet]
        public ActionResult Events()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult Events(ServiceEvents parameterserviceevents, HttpPostedFileBase Event_inputfile)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.Events(parameterserviceevents, Session["Id"].ToString(), Event_inputfile);
                  return RedirectToAction("Agreement", "ServiceAgreement");
                // return RedirectToAction("servicecategories", "Servicedetails");
                //return RedirectToAction("SocialMedia", "Services");
                // < a href = "@Url.Action("SocialMedia", "Services")" >
            }

            return View();
        }

        [HttpGet]
        public ActionResult SocialMedia()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult SocialMedia(ServiceSocialMedia parameterserviceSocialMedia)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.SocialMedia(parameterserviceSocialMedia, Session["Id"].ToString());
                //return RedirectToAction("servicecategories", "Servicedetails");
              //  return RedirectToAction("IMCWebsite", "Services");

                return RedirectToAction("Agreement", "ServiceAgreement");

            }
            return View();
        }

        [HttpGet]
        public ActionResult IMCWebsite()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult IMCWebsite(ServiceIMCWebsite parameterserviceIMCWebsite)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.IMCWebsite(parameterserviceIMCWebsite, Session["Id"].ToString());
                //return RedirectToAction("servicecategories", "Servicedetails");
               // return RedirectToAction("Production", "Services");
                  return RedirectToAction("Agreement", "ServiceAgreement");
               
            }
            return View();
        }

        [HttpGet]
        public ActionResult Production()
        {
            if (Session["Id"] == null)
                return RedirectToAction("servicedetail", "Servicedetails");
            //return RedirectToAction("Index", "User");

            return View();
        }

        [HttpPost]
        public ActionResult Production(ServiceProduction paramserviceProduction)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                objUtility.Production(paramserviceProduction, Session["Id"].ToString());
                 return RedirectToAction("Agreement", "ServiceAgreement");
                //return RedirectToAction("servicecategories", "Servicedetails");
               // return RedirectToAction("HomeGrid", "Home");
            }
            return View();
        }








































    }
}