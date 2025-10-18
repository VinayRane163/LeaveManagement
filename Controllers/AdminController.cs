using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using LeaveManagement.Models;
using System.Drawing;
using System.Reflection;


namespace LeaveManagement
{
    public class AdminController : Controller
    {

        private string ConnectionString;

        public AdminController()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LeaveManagementDB"].ConnectionString;
        }
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult AddManager()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ManagerVM managerVM = new ManagerVM();
            managerVM.ManagerList = Managerlist();
            managerVM.Manager = new Manager();
            return View(managerVM);
        }
        [HttpPost]
        public ActionResult AddManager(ManagerVM M)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                M.ManagerList = Managerlist();
                return View(M); 
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddManager", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", M.Manager.Name);
                    cmd.Parameters.AddWithValue("@Email", M.Manager.Email);
                    cmd.Parameters.AddWithValue("@pass", M.Manager.Password);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        TempData["ResponseMSG"] = "Manager Added Successfully";
                    }
                    else
                    {
                        TempData["ResponseMSG"] = "Error in Adding Manager";
                    }
                }

                catch (Exception ex)
                {
                    TempData["ResponseMSG"] = "Error in Adding Manager";
                }
            }

            return RedirectToAction("AddManager");
        }


        public ActionResult AddEmployee()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
           
            EmployeeVM employeeVM = new EmployeeVM();
            employeeVM.EmployeeList = Employeelist();
            employeeVM.ManagerList = Managerlist();
            employeeVM.Employee = new Employee();
            return View(employeeVM);
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeVM E)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                E.ManagerList = Managerlist();
                E.EmployeeList = Employeelist();
                return View(E); // keep validation messages visible
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddEmployee", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", E.Employee.Name);
                    cmd.Parameters.AddWithValue("@Email", E.Employee.Email);
                    cmd.Parameters.AddWithValue("@pass", E.Employee.Password);
                    cmd.Parameters.AddWithValue("@DOB", E.Employee.DOB );
                    cmd.Parameters.AddWithValue("@ManagerId", E.Employee.ManagerID);
                    int r = cmd.ExecuteNonQuery();
                    if (r > 0)
                    {
                        TempData["ResponseMSG"] = "Employee Added Successfully";
                    }
                    else
                    {
                        TempData["ResponseMSG"] = "Error in Adding Employee";
                    }
                }

                catch (Exception ex)
                {
                    TempData["ResponseMSG"] = "Error in Adding Employee";
                }
            }

            return RedirectToAction("AddEmployee");
        }

        public List<Manager> Managerlist()
        {
            
            List<Manager> ManagerList = new List<Manager>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Manager ", con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Manager i = new Manager();
                    i.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    i.Name = sqlDataReader["Name"].ToString();
                    i.Email = sqlDataReader["Email"].ToString();
                    i.Password = sqlDataReader["Password"].ToString();
                    ManagerList.Add(i);
                }

            }
            return ManagerList;
        }
        public List<Employee> Employeelist()
        {

            List<Employee> EmployeeList = new List<Employee>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT E.ID,E.Name,E.Email,E.DOB,E.Password,E.ManagerID,M.Name as ManagerName FROM Employee E left join Manager M on M.ID=E.ManagerID", con);
                SqlDataReader sqlDataReader = cmd.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Employee i = new Employee();
                    i.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    i.Name = sqlDataReader["Name"].ToString();
                    i.Email = sqlDataReader["Email"].ToString();
                    i.DOB = Convert.ToDateTime(sqlDataReader["DOB"]);
                    i.Password = sqlDataReader["Password"].ToString();
                    i.ManagerID = Convert.ToInt32(sqlDataReader["ManagerId"]);
                    i.ManagerName = sqlDataReader["ManagerName"].ToString();
                    EmployeeList.Add(i);
                }
            }
            return EmployeeList;
        }
    }
}