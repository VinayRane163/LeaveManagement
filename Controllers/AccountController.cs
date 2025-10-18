using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManagement.Controllers
{
    public class AccountController : Controller
    {
        private string ConnectionString;

        public AccountController()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LeaveManagementDB"].ConnectionString;
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password,string LoginAs)
        {
            string AdminID = ConfigurationManager.AppSettings["admin"].ToString();
            string AdminPASS = ConfigurationManager.AppSettings["password"].ToString();
            if (AdminID == UserName && AdminPASS == Password)
            {
                Session["Admin"] = ConfigurationManager.AppSettings["admin"].ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    SqlCommand cmd = new SqlCommand("Login", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Username", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Type", LoginAs);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    int count = Convert.ToInt32(reader["count"].ToString());
                    if (count > 0)
                    {
                        if (LoginAs == "Manager")
                        {
                            Session["Role"] = "Manager";
                            Session["ManagerID"] = reader["Id"].ToString();
                            Session["ManagerName"] = reader["Name"].ToString();
                            return RedirectToAction("Index", "Manager");
                        }
                        else if (LoginAs == "Employee")
                        {
                            Session["Role"] = "Employee";
                            Session["EmployeeID"] = reader["Id"].ToString();
                            Session["EmployeeName"] = reader["Name"].ToString();
                            return RedirectToAction("Index", "Employee");
                        }
                    }
                    else
                    {
                        TempData["ResponseMSG"] = "Invalid Credentials";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ResponseMSG"] = "Invalid Credentials";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}