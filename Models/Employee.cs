using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required]
        public string Password { get; set; }
        public int ManagerID { get; set;}
        public string ManagerName { get; set; }
        public int IsActive { get; set; }
    }
}