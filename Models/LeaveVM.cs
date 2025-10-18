using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagement.Models
{
    public class LeaveVM
    {
        public Leave Leave { get; set; }
        public List<LeaveVM> LeaveVMs { get; set; }
    }
}