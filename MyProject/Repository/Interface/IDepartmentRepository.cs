using MyProject.Models;

namespace MyProject.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> Get();
        Department Get(int ID);
        int Insert(Department department);
        int Update(Department model);
        int Delete(int ID);
    }
}
