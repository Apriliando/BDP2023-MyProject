using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using MyProject.View_Models;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public string GenerateNIK()
        {
            int employeeCount = myContext.Employees.Count() + 1;
            DateTime dateTime = DateTime.UtcNow;
            string dateFormat = dateTime.ToString("ddMMyyyy");
            string NIK = dateFormat + employeeCount.ToString("D3");
            return NIK;
        }
        public int Delete(string NIK)
        {
            //throw new NotImplementedException();
            int save2 = 0;
            Employee employee = myContext.Employees.Where(e => e.NIK == NIK).SingleOrDefault();
            if (employee != null)
            {
                myContext.Employees.Remove(employee);
                save2 = myContext.SaveChanges();
                return save2;
            }
            return save2;
        }

        public IEnumerable<Employee> Get()
        {
            return myContext.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            return myContext.Employees.Find(NIK);
            //return myContext.Employees.FirstOrDefault(e => e.NIK == NIK);
            //return myContext.Employees.Where(e => e.NIK == NIK).FirstOrDefault();
            //return myContext.Employees.Where(e => e.NIK == NIK).SingleOrDefault();
        }

        public IEnumerable<Employee> GetAll()
        {
            return myContext.Employees.ToList();
        }

        public int Insert(Employee employee)
        {
            employee.NIK = GenerateNIK();
            employee.Department = myContext.Departments.Find(employee.Department_ID);
            var emailChecker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute(); //added 13-4-2023
            if (myContext.Employees.Select(e => e.NIK).Contains(employee.NIK))
                return -1;
            if (myContext.Employees.Select(e => e.Phone).Contains(employee.Phone))
                return -2;
            if (Regex.Match(employee.Phone, @"\D+").Success) // added 13-4-2023, if phone match the pattern of regex (non-digit characters)
                return -3;
            if (myContext.Employees.Select(e => e.Email).Contains(employee.Email))
                return -4;
            if (!emailChecker.IsValid(employee.Email)) // added 13-4-2023, if email doesn't contain '@'
                return -5;
            myContext.Employees.Add(employee);
            var save = myContext.SaveChanges();
            return save;
        }

        public async Task <int> Register(RegisterVM registerVM)
        {
            var emailChecker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute(); //added 13-4-2023
            Employee employee = new Employee
            {
                //NIK = GenerateNIK(),
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Gender)registerVM.Gender,
                Department_ID = registerVM.Department_ID
            };
            employee.NIK = GenerateNIK();
            //employee.Department = myContext.Departments.Find(employee.Department_ID); //don't know if needed or not
            if (myContext.Employees.Select(e => e.Phone).Contains(employee.Phone))
                return -2;
            if (Regex.Match(employee.Phone, @"\D+").Success) // added 13-4-2023, if phone match the pattern of regex (non-digit characters)
                return -3;
            if (myContext.Employees.Select(e => e.Email).Contains(employee.Email))
                return -4;
            if (!emailChecker.IsValid(registerVM.Email)) // added 13-4-2023, if email doesn't contain '@'
                return -5;
            myContext.Employees.AddAsync(employee);
            Account account = new Account
            {
                NIK = GenerateNIK(),
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password),
                //Employee = employee //don't know if needed or not
            };
            if (registerVM.Password.Length < 8 || registerVM.Password.Length > 64)
                return -1;
            else
            {
                myContext.Accounts.AddAsync(account);
                var save = await myContext.SaveChangesAsync();
                return save;
            }
        }

        public int Update(Employee model)
        {
            //throw new NotImplementedException();
            int save1 = 0;
            Employee employee = myContext.Employees.Where(e => e.NIK == model.NIK).SingleOrDefault();
            if (employee != null)
            {
                myContext.Entry(employee).CurrentValues.SetValues(model);
                employee.Department = myContext.Departments.Find(model.Department_ID); //added 11-4-2023
                save1 = myContext.SaveChanges();
                return save1;
            }
            return save1;
        }
    }
}
