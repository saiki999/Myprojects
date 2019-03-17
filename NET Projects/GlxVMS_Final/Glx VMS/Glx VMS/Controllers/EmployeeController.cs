using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using VMS.DataAccessLayer;
using VMS.Models;


namespace Glx_VMS.Controllers
{
    public class EmployeeController : Controller
    {

        string exceptionResult;
        public ActionResult Index()
        {
            return View();
        }
        // GET: Employee
        [HttpGet]
        public JsonResult GetEmployeeList()
        {
            DataTable tbEmployee = new DataTable();
            var employeeList = new List<Employee>();

            try
            {
                tbEmployee = DataAccess.GetEmployeeList();

                foreach (DataRow employee in tbEmployee.Rows)
                {
                    var empObj = new Employee
                    {
                        Id = Convert.ToInt32(employee["Id"]),
                        EmployeeId = employee["EmployeeId"].ToString(),
                        EmployeeFirstName = employee["EmployeeFirstName"].ToString(),
                        EmployeeLastName = employee["EmployeeLastName"].ToString(),
                        Department = employee["Department"].ToString(),
                        Location = employee["Location"].ToString(),
                        Email = employee["Email"].ToString(),
                        Phone = employee["Phone"].ToString(),
                        EmployeePhotoUrl = employee["EmployeePhotoUrl"].ToString()
                    };

                    employeeList.Add(empObj);
                }
            }
            catch (Exception ex)
            {
                SendExceptionEmailNotification(ex);
            }


            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }

        // GET: Employee/Details/5
        public JsonResult Details(int id)
        {
            DataTable tbEmployee = new DataTable();
            try
            {
                Employee empObj;
                tbEmployee = DataAccess.GetEmployee(id);

                empObj = new Employee()
                {
                    Id = Convert.ToInt32(tbEmployee.Rows[0].ItemArray[0].ToString()),
                    EmployeeId = tbEmployee.Rows[0].ItemArray[1].ToString(),
                    EmployeeFirstName = tbEmployee.Rows[0].ItemArray[2].ToString(),
                    EmployeeLastName = tbEmployee.Rows[0].ItemArray[3].ToString(),
                    Department = tbEmployee.Rows[0].ItemArray[4].ToString(),
                    Location = tbEmployee.Rows[0].ItemArray[5].ToString(),
                    Email = tbEmployee.Rows[0].ItemArray[6].ToString(),
                    //    Phone = tbEmployee.Rows[0].Field<Double>("Phone"),
                    Phone = tbEmployee.Rows[0].ItemArray[7].ToString(),
                    EmployeePhotoUrl = tbEmployee.Rows[0].ItemArray[8].ToString()

                };
                return Json(empObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SendExceptionEmailNotification(ex);
                return Json("No Record Found", JsonRequestBehavior.AllowGet);
            }

        }


        // POST: Employee/Create
        [HttpPost]
        public JsonResult Create(Employee employee)
        {
            try
            {
                // TODO: Add insert logic here


                return Json(DataAccess.CreateEmployee(employee), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
               
                SendExceptionEmailNotification(ex);
                return Json("Something went wrong");
            }
        }



        // POST: Employee/Edit/5
        [HttpPost]
        public JsonResult Edit(Employee employee)
        {
            try
            {
                // TODO: Add update logic here

                return Json(DataAccess.EditEmployee(employee), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                SendExceptionEmailNotification(ex);
                return Json("Something went wrong");
            }
        }

        [HttpPost]
        public JsonResult Delete(int Id)
        {
            try
            {
                return Json(DataAccess.DeleteEmployee(Id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                SendExceptionEmailNotification(ex);
                return Json("Something went wrong");
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
