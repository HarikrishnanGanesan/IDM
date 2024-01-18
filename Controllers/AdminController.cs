using IDM.Data;
using IDM.Models.Domain;
using IDM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using Newtonsoft.Json;
// using IDM.Filters;

namespace IDM.Controllers
{
    [Authorize(Roles = "Admin")]
    // [LogActionFilter]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        // [HttpGet]
        // public async Task<IActionResult> show()
        // {

        //     var employee = await applicationDbContext.Employee.ToListAsync();
        //     return View(employee);

        // }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
           try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

               using (var client = new HttpClient(clientHandler))
                {
                    client.BaseAddress = new Uri("http://localhost:5156/api/Details");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("http://localhost:5156/api/Details");

                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var employee = JsonConvert.DeserializeObject<List<Employee>>(data);
                        return View(employee);
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine("An error occurred while processing the request: " + ex.Message);
               ViewBag.ErrorMessage = "An error occurred while retrieving the data: " + ex.Message;
                return View("Error");
            }
        }

        [HttpGet]

        public IActionResult add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> add(AddEmployeeViewModel addEmployeeViewModel)
        {
            var exceptions = new List<Exception>();    
            
            try
            {
                var employee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    FirstName = addEmployeeViewModel.FirstName,
                    LastName = addEmployeeViewModel.LastName,
                    email = addEmployeeViewModel.email,
                    userPassword = addEmployeeViewModel.userPassword,
                    Salary = addEmployeeViewModel.Salary,
                    Department = addEmployeeViewModel.Department,
                    DateOfBirth = addEmployeeViewModel.DateOfBirth,
                    AdhaarNumber = addEmployeeViewModel.AdhaarNumber,
                    Address = addEmployeeViewModel.Address,
                    MobileNumber = addEmployeeViewModel.MobileNumber,
                    Gender = addEmployeeViewModel.Gender,
                };

                
                if (string.IsNullOrEmpty(employee.FirstName) || string.IsNullOrEmpty(employee.LastName))
                {
                    throw new ValidationException("First Name and Last Name are required fields.");
                }

                await applicationDbContext.Employee.AddAsync(employee);
                await applicationDbContext.SaveChangesAsync();

                return RedirectToAction("show");
            }
            catch (ValidationException validationException)
            {
                Console.WriteLine(validationException.Message);
                return View("ValidationError");
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> update(Guid id)
        {

            var employee = await applicationDbContext.Employee.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var updateEmployeeViewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    email = employee.email,
                    userPassword = employee.userPassword,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth,
                    AdhaarNumber = employee.AdhaarNumber,
                    Address = employee.Address,
                    MobileNumber = employee.MobileNumber,
                    Gender = employee.Gender,


                };
                return await Task.Run(() => View("Update", updateEmployeeViewModel));
            }

            return View(employee);
        }

        [HttpPost]

        public async Task<IActionResult> update(UpdateEmployeeViewModel updateEmployeeViewModel)
        {
            var exceptions = new List<Exception>();
            try
            {
                var employee = await applicationDbContext.Employee.FindAsync(updateEmployeeViewModel.Id);

                if (employee != null)
                {
                    employee.FirstName = updateEmployeeViewModel.FirstName;
                    employee.LastName = updateEmployeeViewModel.LastName;
                    employee.email = updateEmployeeViewModel.email;
                    employee.userPassword = updateEmployeeViewModel.userPassword;
                    employee.Salary = updateEmployeeViewModel.Salary;
                    employee.DateOfBirth = updateEmployeeViewModel.DateOfBirth;
                    employee.Department = updateEmployeeViewModel.Department;
                    employee.AdhaarNumber = updateEmployeeViewModel.AdhaarNumber;
                    employee.Address = updateEmployeeViewModel.Address;
                    employee.MobileNumber = updateEmployeeViewModel.MobileNumber;
                    employee.Gender = updateEmployeeViewModel.Gender;

                    await applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("show");
                }

                return RedirectToAction("show");
            }

            catch (DbUpdateException dbException)
            {
                Console.WriteLine("Database Error" + dbException.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Updation Error:" + dbException.Message);
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the employee.";
                exceptions.Add(exception);
                return View("Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> delete(UpdateEmployeeViewModel updateEmployeeViewModel)
        {
            var employee = await applicationDbContext.Employee.FindAsync(updateEmployeeViewModel.Id);

            if (employee != null)
            {
                applicationDbContext.Employee.Remove(employee);

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("show");
            }

            return RedirectToAction("show");
        }
    }
}
