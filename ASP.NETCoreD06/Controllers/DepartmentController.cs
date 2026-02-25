using ASP.NETCoreD06.ViewModels.Department;
using ASP.NETCoreD06.Data.Context;
using ASP.NETCoreD06.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreD04.Controllers
{
    public class DepartmentController : Controller
    {
        /*------------------------------------------------------------------*/
        // Context => DB => Data Access
        private readonly AppDbContext db = new AppDbContext();
        /*------------------------------------------------------------------*/
        // Get All Departments
        [HttpGet]
        public IActionResult Index()
        {
            var departmentsReadVM = db.Departments.Select(d => new DepartmentReadVM
            {
                Id = d.Id,
                Name = d.Name
            });
            return View(departmentsReadVM);
        }
        /*------------------------------------------------------------------*/
        // View Details 
        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = db.Departments.Find(id);

            if (department == null)
            {
                return RedirectToAction("Index");
            }

            // Map From Domain Model To View Model
            var departmentReadVM = new DepartmentReadVM
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(departmentReadVM);
        }
        /*------------------------------------------------------------------*/
        // Create New Department
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /*------------------------------------------------------------------*/
        // Create New Department
        [HttpPost]
        public IActionResult Create(DepartmentCreateVM departmentCreateVM)
        {
            var newDepartment = new Department
            {
                Name = departmentCreateVM.Name
            };

            db.Departments.Add(newDepartment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Edit Department
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("Index");
            }

            // Map Domain Model To VM
            var departmentEditVM = new DepartmentEditVM
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(departmentEditVM);
        }
        /*------------------------------------------------------------------*/
        // Edit Department
        [HttpPost]
        public IActionResult Edit(DepartmentEditVM departmentEditVM)
        {
            var departmentInDb = db.Departments.Find(departmentEditVM.Id);
            if (departmentInDb == null)
            {
                return RedirectToAction("Index");
            }

            // Map From VM To Domain Model
            departmentInDb.Name = departmentEditVM.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Delete Department
        public IActionResult Delete(int id)
        {
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("Index");
            }
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
    }
}
