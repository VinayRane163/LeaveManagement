using LeaveManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManagement.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        private string ConnectionString;

        public ManagerController()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LeaveManagementDB"].ConnectionString;
        }
        public ActionResult Index()
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult ApproveLeave()
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select L.ID,L.EmployeeID,e.Name,L.FromDate,L.ToDate,L.Reason,LS.Title " +
                "from Leaves L left join employee e on e.id=l.EmployeeID left join LeaveStatus LS  on LS.ID=L.StatusCode where l.ManagerID=" + Convert.ToInt32(Session["ManagerID"].ToString()) + " and L.StatusCode=0 order by FromDate", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Leave> LeaveList = new List<Leave>();
            while (reader.Read())
            {
                Leave l = new Leave();
                l.Id = Convert.ToInt32(reader["ID"].ToString());
                l.EmployeeId = Convert.ToInt32(reader["EmployeeID"].ToString());
                l.EmployeeName = reader["Name"].ToString();
                l.StartDate = Convert.ToDateTime(reader["FromDate"].ToString());
                l.EndDate = Convert.ToDateTime(reader["ToDate"].ToString());
                l.Reason = reader["Reason"].ToString();
                l.Status = reader["Title"].ToString();
                LeaveList.Add(l);
            }
            return View(LeaveList);
        }
        public ActionResult LeaveStatus()
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select L.ID,L.EmployeeID,e.Name,L.FromDate,L.ToDate,L.Reason,LS.Title from Leaves L left join employee e on e.id=l.EmployeeID left join LeaveStatus LS  on LS.ID=L.StatusCode where l.ManagerID=" + Convert.ToInt32(Session["ManagerID"].ToString()) + " order by FromDate", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Leave> LeaveList = new List<Leave>();
            while (reader.Read())
            {
                Leave l = new Leave();
                l.Id = Convert.ToInt32(reader["ID"].ToString());
                l.EmployeeId = Convert.ToInt32(reader["EmployeeID"].ToString());
                l.EmployeeName = reader["Name"].ToString();
                l.StartDate = Convert.ToDateTime(reader["FromDate"].ToString());
                l.EndDate = Convert.ToDateTime(reader["ToDate"].ToString());
                l.Reason = reader["Reason"].ToString();
                l.Status = reader["Title"].ToString();
                LeaveList.Add(l);
            }
            return View(LeaveList);
        }

        public ActionResult Approve(int id)
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Leaves SET StatusCode = 1 WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    TempData["ResponseMSG"] = "Leave Approved Successfully!";
                }
                else
                {
                    TempData["ResponseMSG"] = "Leave Approval failed!";
                }
            }
            return RedirectToAction("ApproveLeave");
        }


        public ActionResult Reject(int id)
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Leaves SET StatusCode = 2 WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);

                con.Open();
                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    TempData["ResponseMSG"] = "Leave Rejected Successfully!";
                }
                else
                {
                    TempData["ResponseMSG"] = "Leave Rejected failed!";
                }
            }
            return RedirectToAction("ApproveLeave");
        }

        public ActionResult checkSession()
        {
            if (Session["ManagerID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

    }
}
