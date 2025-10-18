using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagement.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int ManagerId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Status { get; set; }  
    }
}