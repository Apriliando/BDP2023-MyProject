using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;

namespace MyProject.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext myContext;
        public DepartmentRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(int ID)
        {
            //throw new NotImplementedException();
            int save2 = 0;
            Department department = myContext.Departments.Where(d => d.ID == ID).SingleOrDefault();
            if (department != null)
            {
                myContext.Departments.Remove(department);
                save2 = myContext.SaveChanges();
                return save2;
            }
            return save2;
        }

        public async Task<IEnumerable<Department>> Get()
        {
            return await myContext.Departments.ToListAsync();
        }

        public Department Get(int ID)
        {
            return myContext.Departments.Find(ID);
        }

        public int Insert(Department department)
        {
            //department.ID = myContext.Departments.Count() + 1;
            myContext.Departments.Add(department);
            var save = myContext.SaveChanges();
            return save;
        }

        public int Update(Department model)
        {
            int save1 = 0;
            Department department = myContext.Departments.Where(d => d.ID == model.ID).SingleOrDefault();
            if (department != null)
            {
                myContext.Entry(department).CurrentValues.SetValues(model);
                save1 = myContext.SaveChanges();
                return save1;
            }
            return save1;
        }
    }
}
