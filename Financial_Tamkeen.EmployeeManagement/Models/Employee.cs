using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financial_Tamkeen.EmployeeManagement.Models
{
    public class Employee
    {
       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Department { get; set; }
        [Required]
        public decimal Salary { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult(
                    "Emploee name cannot empty",
                    new[] { nameof(FirstName), nameof(LastName) });
            }
        }

    }
}
