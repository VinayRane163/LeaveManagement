using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagement.Models
{
    public class EmployeeVM
    {
        public Employee Employee { get; set; }
        public List<Employee> EmployeeList { get; set; }
        public List<Manager> ManagerList { get; set; }
    }
}