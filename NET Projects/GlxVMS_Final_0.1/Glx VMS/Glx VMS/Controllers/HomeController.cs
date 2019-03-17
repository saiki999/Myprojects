using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Mvc;
using VMS.DataAccessLayer;
using VMS.Models;



namespace Glx_VMS.Controllers
{
    public class HomeController : Controller
    {


        public object Keys { get; private set; }
        string exceptionResult;
        public ActionResult Index(string location)
        {
            if (location == null)
            {
                return View("NotFound");
            }
            else
            {
                return View();
            }

        }

        #region Posting Visitor and Send Email Notification to Employee /*Exception Handled */

        [HttpPost]
        public ActionResult PostVisitor(VisitorVisit visitorVisit)

        {
            string result;
            try
            {


                DataAccess.PostVisitor(visitorVisit);

                string empEmailSubject = "Visitor Notification - " + visitorVisit.VisitorFirstName + " " + visitorVisit.VisitorLastName + " " + "is here to meet you";
                result = DataAccess.EmailNotificationToEmployee(visitorVisit, empEmailSubject);
                string visitorEmailSubject = visitorVisit.VisitorFirstName + " " + visitorVisit.VisitorLastName + " " + "Thanks for visiting";
                DataAccess.EmailNotificationToVisitor(visitorVisit, visitorEmailSubject);

            }

            //Send Email To Application owner about expection details from here
            catch (Exception ex)
            {
                SendExceptionEmailNotification(ex);
                return Json(exceptionResult);
            }

            return Json(result);
        }
        #endregion

        #region GetEmployee /* Exception Handelled*/
        [HttpPost]
        public JsonResult GetEmployee(string searchText, string location)
        {
            string employeeDetails = string.Empty;

            try
            {

                DataTable dtemployee = new DataTable();
                dtemployee = DataAccess.GetEmployee(searchText, location);

                employeeDetails = dtemployee.ConvertToJson();
                return Json(employeeDetails, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                SendExceptionEmailNotification(ex);

            }
            return Json(employeeDetails, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region UploadImage /* Exception Handelled*/

        public JsonResult UploadImage(string imageData, string imageFile)
        {
            string[] dataPieces = imageData.Split(',');
            imageData = dataPieces[1];
            //    string imageFile= "Visitor_" + DateTime.Now.Ticks + ".png";
            string fileNameWitPath = Server.MapPath("~/Images/" + imageFile);

            try
            {
                using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(imageData);
                        bw.Write(data);
                        bw.Close();
                    }
                    fs.Close();
                }
            }
            catch (FileNotFoundException ex)
            {

                throw new Exception(string.Format("File with Filename {0} does not exists ", ex.FileName.ToString()));
            }
            catch (Exception ex)
            {
                SendExceptionEmailNotification(ex);

            }
                      

            String test = "Vistor info saved successfully";
            return Json(test, JsonRequestBehavior.AllowGet);
        }

#endregion

        public ActionResult LastPage(string file, string fname, string lname, string company, string purpose, string email, double phone, int hostempid, string empfname, string emplname, string ofcLocation)
        {
            ViewBag.Image = file;
            ViewBag.FirstName = fname;
            ViewBag.LastName = lname;
            ViewBag.Company = company;
            ViewBag.Purpose = purpose;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.HostEmpId = hostempid;
            ViewBag.EmpFirstName = empfname;
            ViewBag.EmpLastName = emplname;
            ViewBag.OfcLocation = ofcLocation;
            return View();
        }




        public void SendExceptionEmailNotification(Exception ex)
        {
            //exceptionResult = ex.Message.ToString();
            exceptionResult = ex.InnerException.ToString() + ex.StackTrace.ToString();

            string recipientEmail = ConfigurationManager.AppSettings["ToEmailAddressOfAppOwner"].ToString();  //Add this key to App Config [<add key="ToEmailAddressOfAppOwner" value="skommanaboina@galaxe.com"/>]
            string senderEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            string smtpServerName = ConfigurationManager.AppSettings["SMTPServerName"].ToString();
            string subject = "Exceptions in Visitor Management Application in Home Controller";
            string emailBody = exceptionResult;
            EmailService.SendEmailNotification(recipientEmail, senderEmail, smtpServerName, subject, emailBody);
        }
    }

}

