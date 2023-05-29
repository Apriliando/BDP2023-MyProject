using MyProject.Models;
using MyProject.View_Models;

namespace MyProject.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        IEnumerable<Employee> GetAll();
        int Insert(Employee employee);
        //int Register(Employee employee, int Department_Id, string Passowrd);
        Task <int> Register(RegisterVM registerVM);
        int Update(Employee model);
        int Delete(string NIK);
    }
}
