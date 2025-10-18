using LeaveManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace LeaveManagement.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private string ConnectionString;

        public EmployeeController()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LeaveManagementDB"].ConnectionString;
        }
        public ActionResult Index()
        {
            if (Session["EmployeeID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult ApplyLeave()
        {
            if (Session["EmployeeID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ApplyLeave(Leave Leave)
        {
            if (Session["EmployeeID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                if (Leave.StartDate > Leave.EndDate)
                {
                    TempData["ResponseMSG"] = "ToDate should be more than FromDate";
                    return View();
                }
                else if(!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    SqlCommand cmd = new SqlCommand("ApplyLeave", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@fromdate", Leave.StartDate);
                    cmd.Parameters.AddWithValue("@ToDate", Leave.EndDate);
                    cmd.Parameters.AddWithValue("@reason",Leave.Reason);
                    cmd.Parameters.AddWithValue("@userid", Session["EmployeeID"].ToString());
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        TempData["ResponseMSG"] = "Leave Application Added Successfully";
                    }
                    else
                    {
                        TempData["ResponseMSG"] = "Error in Adding Leave Application";
                    }
                    return RedirectToAction("LeaveStatus");
                }
            }
            else {
                return View();
            }

        }

        public ActionResult LeaveStatus()
        {
            if (Session["EmployeeID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select L.ID,L.FromDate,L.ToDate,L.Reason,LS.Title from Leaves L left join LeaveStatus LS on LS.ID=L.StatusCode where L.EmployeeID=" + Convert.ToInt32(Session["EmployeeID"].ToString()) + " order by id desc", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Leave> LeaveList = new List<Leave>();
            while (reader.Read())
            {
                Leave l = new Leave();
                l.Id = Convert.ToInt32(reader["ID"].ToString());
                l.StartDate = Convert.ToDateTime(reader["FromDate"].ToString());
                l.EndDate = Convert.ToDateTime(reader["ToDate"].ToString());
                l.Reason = reader["Reason"].ToString();
                l.Status = reader["Title"].ToString();
                LeaveList.Add(l);
            }
            return View(LeaveList);
        }

        public ActionResult checkAdminSession()
        {
            if (Session["EmployeeID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }
    }
}
