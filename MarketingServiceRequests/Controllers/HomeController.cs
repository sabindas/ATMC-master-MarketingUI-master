using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MarketingServiceRequests.Models;

namespace MarketingServiceRequests.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult HomeGrid()
        {
            if (Session["ContactId"] == null)
                return RedirectToAction("Login", "Login");

            //Utility objUtility = new Utility();
            //List<MarketingServiceRequest> lstMSR = objUtility.GetMarketingServiceRequest(Session["ContactId"].ToString());

            //return View(lstMSR);
            return View();
        }


        public ActionResult GetHomeGrid(string word, string sord, int page, int rows, string searchString)
        {
            if (Session["ContactId"] == null)
                return RedirectToAction("Login", "Login");

            Utility objUtility = new Utility();
            List<MarketingServiceRequest> Results = objUtility.GetMarketingServiceRequest(Session["ContactId"].ToString());

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = Results.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            if (sord.ToUpper() == "DESC")
            {
                Results = Results.OrderByDescending(s => s.ProjectTitle).ToList();
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                Results = Results.OrderBy(s => s.ProjectTitle).ToList();
                Results = Results.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = Results
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
