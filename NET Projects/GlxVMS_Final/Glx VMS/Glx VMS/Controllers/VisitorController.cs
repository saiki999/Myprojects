using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string exceptionResult;

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetVisitorsInfo(string selecteddate, int type)
        {

            string visitorDetails;

            DataTable tbVisitor = new DataTable();

            try
            {
                tbVisitor = DataAccess.GetVisitorsInfo(selecteddate, type);

                visitorDetails = tbVisitor.ConvertToJson();

                return Json(visitorDetails, JsonRequestBehavior.AllowGet);
            }

            catch(Exception ex)
            {
                SendExceptionEmailNotification(ex);
                return Json("No Record Found", JsonRequestBehavior.AllowGet);
            }
        }

        public void SendExceptionEmailNotification(Exception ex)
        {
            //exceptionResult = ex.Message.ToString();
            exceptionResult = ex.InnerException.ToString() + ex.StackTrace.ToString();

            string recipientEmail = ConfigurationManager.AppSettings["ToEmailAddressOfAppOwner"].ToString();  //Add this key to App Config [<add key="ToEmailAddressOfAppOwner" value="skommanaboina@galaxe.com"/>]
            string senderEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            string smtpServerName = ConfigurationManager.AppSettings["SMTPServerName"].ToString();
            string subject = "Exceptions in Visitor Management Application in Employee Controller";
            string emailBody = exceptionResult;
            EmailService.SendEmailNotification(recipientEmail, senderEmail, smtpServerName, subject, emailBody);
        }

    }
}