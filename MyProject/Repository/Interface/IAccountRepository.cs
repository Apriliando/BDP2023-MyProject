using MyProject.Models;
using MyProject.View_Models;

namespace MyProject.Repository.Interface
{
    public interface IAccountRepository
    {
        IEnumerable<Account> Get();
        Account Get(string NIK);
        Task<int> Login(LoginVM loginVM);
        int Insert(Account account);
        int Update(Account model);
        int Delete(string NIK);
    }
}
