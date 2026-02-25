using ASP.NETCoreD06.Data.Context;
using ASP.NETCoreD06.Models;
using ASP.NETCoreD06.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD06.Controllers
{
    public class EmployeeController : Controller
    {
        /*------------------------------------------------------------------*/
        // Context => DB => Data Access
        private readonly AppDbContext db = new AppDbContext();
        /*------------------------------------------------------------------*/
        // Index => List All => Main Action => Landing Page
        [HttpGet]
        public IActionResult Index()
        {
            // Get All Employees
            // Map From Domain Model To VM
            var employeesReadVM = db.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeReadVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Salary = e.Salary,
                    ImageURL = e.ImageURL,
                    Department = e.Department!.Name
                }).ToList();

            return View(employeesReadVM);
        }
        /*------------------------------------------------------------------*/
        // View Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = db.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            // Map FromDomain Model To View Model
            var employeeReadVM = new EmployeeReadVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                ImageURL = employee.ImageURL,
                Department = employee.Department!.Name
            };

            return View(employeeReadVM);
        }
        /*------------------------------------------------------------------*/
        // Create Employee
        [HttpGet]
        public IActionResult Create()
        {
            var employeeCreateVM = new EmployeeCreateVM
            {
                Departments = GetDepartmentsForDropDown()
            };
            return View(employeeCreateVM);
        }
        /*------------------------------------------------------------------*/
        // Create Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeCreateVM employeeCreateVM)
        {
            if (!ModelState.IsValid)
            {
                employeeCreateVM.Departments = GetDepartmentsForDropDown();
                return View(employeeCreateVM);
            }

            // Create Unique Name For Image
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeCreateVM.Image.FileName);

            // Define Path To Save Image
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                "Employees");

            // Create Folder If Not Exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                employeeCreateVM.Image.CopyTo(stream);
            }

            // Map From VM To Domain Model
            var employee = new Employee
            {
                Name = employeeCreateVM.Name,
                Age = employeeCreateVM.Age,
                Salary = employeeCreateVM.Salary,
                ImageURL = uniqueFileName,
                DepartmentId = employeeCreateVM.DepartmentId
            };

            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Edit Employee
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = db.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            // Map Domain Model To VM
            var employeeEditVM = new EmployeeEditVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department!.Name,
                Departments = GetDepartmentsForDropDown()
            };
            return View(employeeEditVM);
        }
        /*------------------------------------------------------------------*/
        // Edit Employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeEditVM employeeEditVM)
        {
            var employeeInDb = db.Employees.FirstOrDefault(e => e.Id == employeeEditVM.Id);
            if (employeeInDb == null)
            {
                return RedirectToAction("Index");
            }

            // Map From VM To Domain Model
            employeeInDb.Name = employeeEditVM.Name;
            employeeInDb.Age = employeeEditVM.Age;
            employeeInDb.Salary = employeeEditVM.Salary;
            employeeInDb.DepartmentId = employeeEditVM.DepartmentId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Delete Employee
        public IActionResult Delete(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Helper Method
        // DRY => Reusable Code => Don't Repeat Yourself
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
    }
}
