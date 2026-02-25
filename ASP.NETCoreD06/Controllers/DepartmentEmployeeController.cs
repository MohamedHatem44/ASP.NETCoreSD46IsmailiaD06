using ASP.NETCoreD06.Data.Context;
using ASP.NETCoreD06.ViewModels.DepartmentEmployee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD06.Controllers
{
    public class DepartmentEmployeeController : Controller
    {
        /*------------------------------------------------------------------*/
        AppDbContext db = new AppDbContext();
        /*------------------------------------------------------------------*/
        public IActionResult DepartmentEmployees()
        {
            var departmentEmployeesVM = new DepartmentEmployeesVM
            {
                Departments = GetDepartmentsForDropDown()
            };
            return View(departmentEmployeesVM);
        }
        /*------------------------------------------------------------------*/
        private List<SelectListItem> GetDepartmentsForDropDown()
        {
            return db.Departments
             .Select(d => new SelectListItem
             {
                 Value = d.Id.ToString(),
                 Text = d.Name
             }).ToList();
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult GetEmployeesByDepartment(int deptId)
        {
            var employees = db.Employees
                .Include(e=>e.Department)
                .Where(e => e.DepartmentId == deptId)
                .Select(e=> new DepartmentEmployeeReadVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Salary = e.Salary,
                    Department = e.Department.Name
                }).ToList();


            return PartialView("_EmployeesPartial", employees); // No Layout
            //return View("_EmployeesPartial", employees);  // Layout
        }
        /*------------------------------------------------------------------*/
    }
}
