# ASP.NETCoreSD46IsmailiaD06

# 🔷 ASP.NET Core MVC Employee & Department Management (.NET 9)

A complete ASP.NET Core MVC project demonstrating:

- ✅ CRUD Operations
- ✅ File Upload using IFormFile
- ✅ Entity Framework Core
- ✅ ViewModels
- ✅ AJAX with Partial Views
- ✅ Route Constraints
- ✅ Client-side & Server-side Validation

---

# 🚀 Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Razor Views
- Bootstrap
- jQuery
- AJAX
- LINQ

---

## 📁 Project Structure

```
ASP.NETCoreD06
│
├── Controllers
│   ├── DepartmentController.cs
│   ├── DepartmentEmployeeController.cs
│   ├── RouteController.cs
│   └── EmployeeController.cs
│
├── Models
│   ├── Department.cs
│   └── Employee.cs
│
├── ViewModels
│   ├── Department
│   │   ├── DepartmentCreateVM.cs
│   │   ├── DepartmentEditVM.cs
│   │   └── DepartmentReadVM.cs
│   ├── DepartmentEmployee
│   │   ├── DepartmentEmployeeReadVM.cs
│   │   └── DepartmentEmployeesVM.cs
│   └── Employee
│       ├── EmployeeCreateVM.cs
│       ├── EmployeeEditVM.cs
│       └── EmployeeReadVM.cs
│
├── Data
│   ├── Context
│   │   └── AppDbContext.cs
│   └── Configuration
│       └── EmployeeConfiguration.cs
├── wwwroot
│   └── Images
│       └── Employees
└── Views
        ├── Department
        ├── DepartmentEmployee
        └── Employee
```

---

# 📂 Project Architecture

The project follows clean separation of concerns:

- **Models** → Represent database tables
- **ViewModels** → Used for UI data transfer
- **Controllers** → Handle logic and requests
- **Views** → Render UI
- **wwwroot** → Static files (Images, CSS, JS)

---

# 🗄️ Database Models

## Department Model

```csharp
public class Department
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
        = new HashSet<Employee>();
}
```

---

## Employee Model

```csharp
public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string? ImageURL { get; set; }

    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; } = null!;
}
```

---

# 📦 ViewModels

## EmployeeCreateVM

Used when creating a new employee.

```csharp
public class EmployeeCreateVM
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
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

    public List<SelectListItem>? Departments { get; set; }
}
```

---

## EmployeeReadVM

```csharp
public class EmployeeReadVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string? ImageURL { get; set; }
    public string Department { get; set; }
}
```

---

# 🎯 Important Concept – File Upload

When uploading files, your form MUST include:

```html
<form enctype="multipart/form-data">
```

### Why?

Default form encoding:
```
application/x-www-form-urlencoded
```

This works for:
- Text
- Numbers
- Select inputs

❌ But NOT for files.

### multipart/form-data

- Sends file as binary
- Splits data into multiple parts
- Allows large file uploads

Without it:
- IFormFile will be null
- File size will be 0
- Model binding fails

---

# 🎮 Controllers

---

# 👨‍💻 EmployeeController

Handles CRUD operations for employees.

---

## Create (GET)

```csharp
public IActionResult Create()
{
    var vm = new EmployeeCreateVM
    {
        Departments = GetDepartmentsForDropDown()
    };

    return View(vm);
}
```

---

## Create (POST)

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(EmployeeCreateVM employeeCreateVM)
{
    if (!ModelState.IsValid)
    {
        employeeCreateVM.Departments = GetDepartmentsForDropDown();
        return View(employeeCreateVM);
    }

    var uniqueFileName = Guid.NewGuid().ToString() +
        Path.GetExtension(employeeCreateVM.Image.FileName);

    string folderPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "Images",
        "Employees");

    if (!Directory.Exists(folderPath))
        Directory.CreateDirectory(folderPath);

    string filePath = Path.Combine(folderPath, uniqueFileName);

    using var stream = new FileStream(filePath, FileMode.Create);
    employeeCreateVM.Image.CopyTo(stream);

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
```

---

# 🏢 DepartmentController

Handles department CRUD.

```csharp
[HttpPost]
public IActionResult Create(DepartmentCreateVM vm)
{
    var department = new Department
    {
        Name = vm.Name
    };

    db.Departments.Add(department);
    db.SaveChanges();

    return RedirectToAction("Index");
}
```

---

# 🔄 DepartmentEmployeeController (AJAX Filtering)

```csharp
[HttpGet]
public IActionResult GetEmployeesByDepartment(int deptId)
{
    var employees = db.Employees
        .Include(e => e.Department)
        .Where(e => e.DepartmentId == deptId)
        .Select(e => new DepartmentEmployeeReadVM
        {
            Id = e.Id,
            Name = e.Name,
            Age = e.Age,
            Salary = e.Salary,
            Department = e.Department.Name
        }).ToList();

    return PartialView("_EmployeesPartial", employees);
}
```

---

# 🧩 Partial View (_EmployeesPartial.cshtml)

```html
@model List<DepartmentEmployeeReadVM>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Age</th>
            <th>Salary</th>
            <th>Department</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var emp in Model)
        {
            <tr>
                <td>@emp.Id</td>
                <td>@emp.Name</td>
                <td>@emp.Age</td>
                <td>@emp.Salary</td>
                <td>@emp.Department</td>
            </tr>
        }
    </tbody>
</table>
```

---

# ⚡ AJAX Script

```javascript
$(document).ready(function () {

    $("#deptDropDown").change(function () {

        var deptId = $(this).val();

        if (deptId !== "") {

            $.ajax({
                url: '/DepartmentEmployee/GetEmployeesByDepartment',
                type: 'GET',
                data: { deptId: deptId },
                success: function (result) {
                    $("#employeesTable").html(result);
                },
                error: function () {
                    alert("Error fetching employees");
                    $("#employeesTable").html("");
                }
            });

        } else {
            $("#employeesTable").html("");
        }

    });

});
```

---

# 🛠 Setup Instructions

1. Clone repository

```bash
git clone <repo-url>
```

2. Configure connection string in `appsettings.json`

3. Run migrations

```bash
dotnet ef database update
```

4. Run application

```bash
dotnet run
```

---

---

# ⭐ Key Features

- Full Employee CRUD
- Image Upload
- Server-side Validation
- Client-side Validation
- AJAX Filtering
- Clean ViewModel Usage
- Route Constraints
- DRY Principles

---

# 👨‍💻 Author

Mohamed Hatem  
Software Engineer

---
