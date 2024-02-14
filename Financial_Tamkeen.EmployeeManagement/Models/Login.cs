using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financial_Tamkeen.EmployeeManagement.Models
{
    public class login
    {
        public String Email { get; set; }

        public String Password { get; set; }
    }

}


