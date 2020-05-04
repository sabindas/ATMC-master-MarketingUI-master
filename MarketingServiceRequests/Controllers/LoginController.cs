using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarketingServiceRequests.Models;

namespace MarketingServiceRequests.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        //[ActionName("Login")]
        public ActionResult Login(Login paramlogin)
        {
            if (ModelState != null && ModelState.IsValid)
            {
                Utility objUtility = new Utility();
                Contact objContact = objUtility.IsValidUser(paramlogin);
                if (objContact.ContactId != null && objContact.Fullname != null)
                {
                    Session["UserName"] = objContact.Fullname;
                    Session["ContactId"] = objContact.ContactId;
                    return RedirectToAction("HomeGrid", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid User Credentials.");
                    return View(paramlogin);
                }

                //ModelState.AddModelError("", "user name is duplicate");
            }
            else {
                //ModelState.AddModelError(string.Empty, "Invalid User Credentials.");
                return View(paramlogin);
            }           
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }
    }
}