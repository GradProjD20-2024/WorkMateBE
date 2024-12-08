using WorkMateBE.Dtos.AccountDto;
using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IAccountRepository
    {
        bool CreateAccount(Account account);
        bool UpdateAccount(int accountId, Account account);
        bool DeleteAccount(int accountId);
        ICollection<Account> GetAllAccounts();
        Account GetAccountById(int accountId);
        Account GetAccountByEmail(string email);
        Account GetAccountByEmployeeId(int employeeId);
        bool CheckEmail(string email);
        bool Login(AccountLogin accountLogin);
        bool CheckEmployee(int employeeId);
        bool ChangePassword(int accountId, string newPassword);
        string ResetPassword(int accountId);
    }
}
