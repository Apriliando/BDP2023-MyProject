using MyProject.Context;
using MyProject.Models;
using MyProject.Repository.Interface;
using BCrypt.Net;
using MyProject.View_Models;
using Microsoft.EntityFrameworkCore;

namespace MyProject.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext myContext;
        public AccountRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(string NIK)
        {
            int save2 = 0;
            Account account = myContext.Accounts.Find(NIK);
            if (account != null)
            {
                myContext.Accounts.Remove(account);
                save2 = myContext.SaveChanges();
                return save2;
            }
            return save2;
        }

        public IEnumerable<Account> Get()
        {
            return myContext.Accounts.ToList();
        }

        public Account Get(string NIK)
        {
            return myContext.Accounts.Find(NIK);
        }
        public Employee GetEmployeeByEmail(string Email)
        {
            return myContext.Employees.Where(e => e.Email == Email).SingleOrDefault();
        }

        public int Insert(Account account)
        {
            int save = 0;
            if(account.NIK.Length < 1)
                save = -1;
            else
            {
                var nikList1 = myContext.Accounts.Select(a => a.NIK).ToList();
                if (nikList1.Contains(account.NIK))
                    save = -2;
                else
                {
                    var nikList2 = myContext.Employees.Select(e => e.NIK).ToList();
                    if (!nikList2.Contains(account.NIK))
                        save = -3;
                    else
                    {
                        account.Employee = myContext.Employees.Find(account.NIK);
                        myContext.Accounts.Add(account);
                        save = myContext.SaveChanges();
                    }
                }
            }
            return save;

        }

        public async Task<int> Login(LoginVM loginVM)
        {
            var state = 0;
            Employee employee = await myContext.Employees.Where(e => e.Email == loginVM.Email).SingleOrDefaultAsync();
            if (employee == null)
                state = -1;
            else
            {
                Account account = await myContext.Accounts.FindAsync(employee.NIK);
                if (account == null)
                    state = -2;
                else
                {
                    if (!BCrypt.Net.BCrypt.Verify(loginVM.Password, account.Password))
                        state = -3;
                    else
                        state = 1;
                }
            }
            return state;
        }

        public int Update(Account model)
        {
            int save1 = 0;
            Account account = myContext.Accounts.Where(a => a.NIK == model.NIK).SingleOrDefault();
            if(account != null)
            {
                myContext.Entry(account).CurrentValues.SetValues(model);
                save1 = myContext.SaveChanges();
                return save1;
            }
            return save1;
        }
    }
}
