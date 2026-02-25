using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD06.ViewModels.Employee
{
    public class EmployeeCreateVM
    {
        /*------------------------------------------------------------------*/
        #region Get From Form
        [MinLength(3)]
        [MaxLength(20)]
        [Required(ErrorMessage ="Name is mandatory")]
        public string Name { get; set; }

        [Required]
        [Range(20, 50)]
        public int Age { get; set; }

        [Required]
        [Range(1000, 5000)]
        public decimal Salary { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        #endregion
        /*------------------------------------------------------------------*/
        #region Send To Form
        public List<SelectListItem>? Departments { get; set; }
        #endregion
        /*------------------------------------------------------------------*/
    }
}
