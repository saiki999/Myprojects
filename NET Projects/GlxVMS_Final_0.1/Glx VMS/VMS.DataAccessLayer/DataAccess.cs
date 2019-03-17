using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using VMS.Models;


namespace VMS.DataAccessLayer
{
    public static class DataAccess
    {
        static string GxVisitor_Conn = ConfigurationManager.ConnectionStrings["GxVisitor_Conn"].ConnectionString;
        static string Emp_Email;
        static string logMessage = string.Empty;

        #region PostVisitor
        public static void PostVisitor(VisitorVisit visitorVisit)
        {
            Visitor visitor = new Visitor
            {
                VisitorFirstName = visitorVisit.VisitorFirstName,
                VisitorLastName = visitorVisit.VisitorLastName,
                Phone = visitorVisit.Phone,
                Email = visitorVisit.Email,
                Company = visitorVisit.Company,
                ImageURL = visitorVisit.ImageURL
            };

            Visit visit = new Visit
            {
                OfficeLocation = visitorVisit.OfficeLocation = "Detroit",
                Purpose = visitorVisit.Purpose,
                HostEmployeeId = visitorVisit.HostEmployeeId
            };

            using (SqlConnection con = new SqlConnection(GxVisitor_Conn))
            {

                SqlCommand sqlCmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCmd.Connection = con;
                sqlCmd.CommandText = "spCreateVisitorVisit";

                sqlCmd.Parameters.Add("@VisitorFirstName", SqlDbType.VarChar).Value = visitor.VisitorFirstName;
                sqlCmd.Parameters.Add("@VisitorLastName", SqlDbType.VarChar).Value = visitor.VisitorLastName;
                sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = visitor.Email;
                sqlCmd.Parameters.Add("@Company", SqlDbType.VarChar).Value = visitor.Company;
                sqlCmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = visitor.Phone;

                sqlCmd.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = visitor.ImageURL;

                sqlCmd.Parameters.Add("@HostEmployeeId", SqlDbType.VarChar).Value = visit.HostEmployeeId;

                sqlCmd.Parameters.Add("@OfficeLocation", SqlDbType.VarChar).Value = visit.OfficeLocation;

                sqlCmd.Parameters.Add("@Purpose", SqlDbType.VarChar).Value = visit.Purpose;

                sqlCmd.Parameters.Add("@VisitorCheckInTime", SqlDbType.VarChar).Value = System.DateTime.Now;
                sqlCmd.Parameters.Add("@VisitorCheckOutTime", SqlDbType.VarChar).Value = System.DateTime.Now.AddMinutes(30);
                try
                {
                    con.Open();
                    sqlCmd.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
        }
        #endregion

        #region EmailNotification
        public static string EmailNotificationToEmployee(VisitorVisit visitorVisit, string subject)
        {


            using (SqlConnection con = new SqlConnection(GxVisitor_Conn))
            {
                SqlCommand sqlCmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCmd.Connection = con;
                sqlCmd.CommandText = "spEmployeeSearchById";
                sqlCmd.Parameters.Add("@id", SqlDbType.Int).Value = visitorVisit.HostEmployeeId;


                try
                {
                    con.Open();
                    Emp_Email = sqlCmd.ExecuteScalar().ToString();
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                //Email Notification to Employee letting him know of Visitor waiting 

                try
                {
                    var senderEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                    string path = ConfigurationManager.AppSettings["EmailBodyFilepath"].ToString();
                    string smtpServerName = ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                    string emailBody = EmailService.GetEmployeeEmailBody(path, visitorVisit);
                    EmailService.SendEmailNotification(Emp_Email, senderEmail, smtpServerName, subject, emailBody);
                    logMessage = logMessage + senderEmail + path + smtpServerName;
                }
                catch (Exception ex)
                {
                    logMessage = logMessage + ex.InnerException;
                    throw ex;
                }

                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }


            return logMessage;
        }



        public static string EmailNotificationToVisitor(VisitorVisit visitorVisit, string visitorEmailSubject)
        {

            try
            {
                var senderEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                string path = ConfigurationManager.AppSettings["VisitorEmailBodyFilepath"].ToString();
                string smtpServerName = ConfigurationManager.AppSettings["SMTPServerName"].ToString();
                string visitoremailBody = EmailService.GetVisitorEmailBody(path, visitorVisit);
                EmailService.SendEmailNotification(visitorVisit.Email, senderEmail, smtpServerName, visitorEmailSubject, visitoremailBody);
                logMessage = logMessage + senderEmail + path + smtpServerName;
            }
            catch (Exception ex)
            {
                logMessage = logMessage + ex.InnerException;
                throw ex;
            }


            return logMessage;
        }
        #endregion

        #region GetEmployee

        public static DataTable GetEmployee(string searchText, string location)
        {
            string employeeDetails;
            DataTable table = new DataTable();

            if (string.IsNullOrEmpty(searchText))
                employeeDetails = string.Empty;
            else
            {
                try
                {

                    using (SqlConnection cn = new SqlConnection(GxVisitor_Conn))
                    {

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spEmployeeSearch";
                            cmd.Parameters.Add("@text", SqlDbType.NVarChar).Value = searchText;
                            cmd.Parameters.Add("@location", SqlDbType.NVarChar).Value = location;
                            cn.Open();
                            table.Load(cmd.ExecuteReader());

                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return table;
        }

        #endregion

        #region GetVisitorInfo
        public static DataTable GetVisitorsInfo(string selecteddate, int type)
        {

            DataTable table = new DataTable();
            try
            {

                using (SqlConnection cn = new SqlConnection(GxVisitor_Conn))
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetVisitorDetails";
                        cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = selecteddate;
                        cmd.Parameters.Add("@type", SqlDbType.Int).Value = type;

                        cn.Open();

                        table.Load(cmd.ExecuteReader());
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return table;
        }
        #endregion


        #region GetEmployeeList /*For Reporting Purpose*/
        public static DataTable GetEmployeeList()
        {
            DataTable tbEmployee = new DataTable();

            try
            {

                using (SqlConnection cn = new SqlConnection(GxVisitor_Conn))
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spEmployeeList";
                        cn.Open();
                        tbEmployee.Load(cmd.ExecuteReader());

                    }
                  
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"execption in GetEmployeeList: {0}", ex.InnerException);
            }
            return tbEmployee;
        }

        #endregion

        public static DataTable GetEmployee(int id)
        {
            DataTable tbEmployee = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(GxVisitor_Conn))
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from Employee where id =" + id;
                        cn.Open();

                        tbEmployee.Load(cmd.ExecuteReader());

                    }
                }
            }

            catch(Exception ex)
            {
                throw new Exception($"execption in GetEmployee: {0}", ex.InnerException);
            }
            return tbEmployee;
        }

        public static int CreateEmployee(Employee employee)
        {
            int i;
            Employee emp = new Employee
            {
                EmployeeId = employee.EmployeeId,
                EmployeeFirstName = employee.EmployeeFirstName,
                EmployeeLastName = employee.EmployeeLastName,
                Department = employee.Department,
                Location = employee.Location,
                Email = employee.Email,
                Phone = employee.Phone,


                EmployeePhotoUrl = employee.EmployeePhotoUrl
            };

            using (SqlConnection con = new SqlConnection(GxVisitor_Conn))
            {

                SqlCommand sqlCmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCmd.Connection = con;
                sqlCmd.CommandText = "spInsertUpdateEmployee";

                sqlCmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = emp.Id;
                sqlCmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar).Value = emp.EmployeeId;
                sqlCmd.Parameters.Add("@EmployeeFirstName", SqlDbType.VarChar).Value = emp.EmployeeFirstName;
                sqlCmd.Parameters.Add("@EmployeeLastName", SqlDbType.VarChar).Value = emp.EmployeeLastName;
                sqlCmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = emp.Department;
                sqlCmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = emp.Location;
                sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = emp.Email;
                sqlCmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = emp.Phone;

                sqlCmd.Parameters.Add("@EmployeePhotoUrl", SqlDbType.VarChar).Value = emp.EmployeePhotoUrl;


                try
                {
                    con.Open();
                    i = sqlCmd.ExecuteNonQuery();
                    return i;
                }
                catch(SqlException ex1)
                {
                    throw ex1;
                }

                catch (Exception ex)
                {
                    throw new Exception($"execption in CreateEmployee: {0}", ex.InnerException );
                }

                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }

        }

        public static int EditEmployee(Employee employee)
        {
            int i;
            Employee emp = new Employee
            {
                Id = employee.Id,
                EmployeeId = employee.EmployeeId,
                EmployeeFirstName = employee.EmployeeFirstName,
                EmployeeLastName = employee.EmployeeLastName,
                Department = employee.Department,
                Location = employee.Location,
                Email = employee.Email,
                Phone = employee.Phone,


                EmployeePhotoUrl = employee.EmployeePhotoUrl
            };

            using (SqlConnection con = new SqlConnection(GxVisitor_Conn))
            {

                SqlCommand sqlCmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCmd.Connection = con;
                sqlCmd.CommandText = "spInsertUpdateEmployee";

                sqlCmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = emp.Id;
                sqlCmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar).Value = emp.EmployeeId;
                sqlCmd.Parameters.Add("@EmployeeFirstName", SqlDbType.VarChar).Value = emp.EmployeeFirstName;
                sqlCmd.Parameters.Add("@EmployeeLastName", SqlDbType.VarChar).Value = emp.EmployeeLastName;
                sqlCmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = emp.Department;
                sqlCmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = emp.Location;
                sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = emp.Email;
                sqlCmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = emp.Phone;

                sqlCmd.Parameters.Add("@EmployeePhotoUrl", SqlDbType.VarChar).Value = emp.EmployeePhotoUrl;

                
                try
                {
                    con.Open();
                    i = sqlCmd.ExecuteNonQuery();
                    return i;
                }

                catch (Exception ex)
                {
                    throw new Exception($"execption in EditEmployee: {0}", ex.InnerException);
                }

                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }



        }

        public static int DeleteEmployee(int Id)
        {


            int i;
            try
            {
                using (SqlConnection con = new SqlConnection(GxVisitor_Conn))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("spDeleteEmployee", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id", Id);
                    i = com.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"execption in DeleteEmployee: {0}",ex.InnerException );
            }
            return i;



        }

    }
}
