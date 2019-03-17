using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VMS.DataAccessLayer;
using VMS.Models;


namespace Glx_VMS.Controllers
{
    public class VisitorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetVisitorsInfo(string selecteddate, int type)
        {

            string visitorDetails;
            DataTable tbVisitor = new DataTable();

            tbVisitor = DataAccess.GetVisitorsInfo(selecteddate, type);

            visitorDetails = tbVisitor.ConvertToJson();

            return Json(visitorDetails, JsonRequestBehavior.AllowGet);
        }

    }
}