using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagement.Models
{
    public class ManagerVM
    {
        public Manager Manager { get; set; }
        public List<Manager> ManagerList { get; set; }
    }
}