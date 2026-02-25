using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD06.ViewModels.DepartmentEmployee
{
    public class DepartmentEmployeesVM
    {
        /*------------------------------------------------------------------*/
        [Required]
        public int DepartmentId { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        /*------------------------------------------------------------------*/
    }
}
