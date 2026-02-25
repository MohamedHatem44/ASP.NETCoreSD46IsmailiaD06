using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD06.ViewModels.Department
{
    public class DepartmentCreateVM
    {
        /*------------------------------------------------------------------*/
        [Display(Name = "Department Name")]
        public required string Name { get; set; }
        /*------------------------------------------------------------------*/
    }
}
