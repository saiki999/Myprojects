using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Text;

namespace VMS.Models
{
    public static class EmailService
    {

        #region GetEmployeeEmailBodyFromTextFile  /* Exception Handelled*/

        public static string GetEmployeeEmailBody(string path, VisitorVisit visitorVisit)
        {

            StringBuilder readBodyFile = new StringBuilder();
            var sr = new StreamReader(path);
            try
            {
                readBodyFile.Append(sr.ReadToEnd());
                readBodyFile.Replace("{0}", visitorVisit.EmployeeFirstName);
                readBodyFile.Replace("{1}", visitorVisit.VisitorFirstName);
                readBodyFile.Replace("{2}", visitorVisit.VisitorLastName);
                readBodyFile.Replace("{3}", visitorVisit.Company);
                readBodyFile.Replace("{4}", visitorVisit.Purpose);
                readBodyFile.Replace("{5}", visitorVisit.ImageURL);
                readBodyFile.Replace("{6}", visitorVisit.OfficeLocation);

            }
            catch (FileNotFoundException ex)
            {

                throw new Exception(string.Format("File with Filename {0} does not exists ", ex.FileName.ToString()));
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Exception in GetEmployeeEmailBody_during FileRead ", ex.Message));
            }
            finally
            {
                sr.Close();
            }


            return readBodyFile.ToString();
        }
        #endregion

        public static string GetVisitorEmailBody(string path, VisitorVisit visitorVisit)
        {
            StringBuilder readBodyFile = new StringBuilder();
            var sr = new StreamReader(path);
            try
            {
                readBodyFile.Append(sr.ReadToEnd());
                readBodyFile.Replace("{0}", visitorVisit.VisitorFirstName);
                readBodyFile.Replace("{1}", visitorVisit.EmployeeFirstName);
                readBodyFile.Replace("{2}", visitorVisit.EmployeeLastName);
                readBodyFile.Replace("{6}", visitorVisit.OfficeLocation);

            }
            catch (FileNotFoundException ex)
            {

                throw new Exception(string.Format("File with Filename {0} does not exists ", ex.FileName.ToString()));
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("Exception in GetEmployeeEmailBody_during FileRead ", ex.Message));
            }
            finally
            {
                sr.Close();
            }


            return readBodyFile.ToString();
        }


        #region Generic SendEmailService /* Exception Handelled*/

        public static string SendEmailNotification(string recipientEmail, string senderEmail, string smtpServerName, string subject, string emailBody)
        {
            var mailMessage = new MailMessage();


            mailMessage.To.Add(recipientEmail);
            mailMessage.From = new MailAddress(senderEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = emailBody;
            mailMessage.IsBodyHtml = true;
            //mailMessage.Attachments;
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpServerName);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("exception while sending email{0}:", ex.InnerException.Message.ToString()));
            }


            return "Email Is Sent";
        }

        #endregion
    }
}
