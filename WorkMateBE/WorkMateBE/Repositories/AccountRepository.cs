using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using Microsoft.EntityFrameworkCore;
using WorkMateBE.Dtos.AccountDto;

namespace WorkMateBE.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        // Kiểm tra xem email đã tồn tại chưa
        public bool CheckEmail(string email)
        {
            return _context.Accounts.Any(a => a.Email == email);
        }

        // Tạo mới tài khoản
        public bool CreateAccount(Account account)
        {
            _context.Add(account);
            return Save();
        }

        // Xóa tài khoản
        public bool DeleteAccount(int accountId)
        {
            var account = GetAccountById(accountId);
            if (account == null)
                return false;

            _context.Remove(account);
            return Save();
        }

        public Account GetAccountByEmail(string email)
        {
            return _context.Accounts.FirstOrDefault(a => a.Email == email);
        }

        // Lấy tài khoản theo ID
        public Account GetAccountById(int accountId)
        {
            return _context.Accounts.FirstOrDefault(a => a.Id == accountId);
        }

        // Lấy tất cả tài khoản
        public ICollection<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public bool Login(AccountLogin accountLogin)
        {
            if (!CheckEmail(accountLogin.Email))
            {
                return false;
            }
            var account = _context.Accounts.Where(p => p.Email == accountLogin.Email).FirstOrDefault();
            if(account.Password != accountLogin.Password)
            {
                return false;
            }
            return true;
        }

        // Cập nhật tài khoản
        public bool UpdateAccount(int accountId, Account account)
        {
            var existingAccount = GetAccountById(accountId);
            if (existingAccount == null)
                return false;

            // Cập nhật các trường
            existingAccount.Email = account.Email;
            existingAccount.Password = account.Password;
            existingAccount.Role = account.Role;
            existingAccount.AvatarUrl = account.AvatarUrl;
            existingAccount.FaceUrl = account.FaceUrl;
            existingAccount.Status = account.Status;
            existingAccount.EmployeeId = account.EmployeeId;

            _context.Update(existingAccount);
            return Save();
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
