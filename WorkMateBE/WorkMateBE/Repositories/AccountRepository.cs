using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;
using Microsoft.EntityFrameworkCore;
using WorkMateBE.Dtos.AccountDto;
using BCrypt.Net;

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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.Password = hashedPassword;
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
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            return account;
        }

        // Lấy tài khoản theo ID
        public Account GetAccountById(int accountId)
        {
            var account = _context.Accounts.Find(accountId);
            return account;
        }

        // Lấy tất cả tài khoản
        public ICollection<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public bool Login(AccountLogin accountLogin)
        {
            // Kiểm tra email có tồn tại hay không
            if (!CheckEmail(accountLogin.Email))
            {
                return false;
            }

            // Lấy thông tin tài khoản theo email
            var account = _context.Accounts.FirstOrDefault(p => p.Email == accountLogin.Email);

            // Kiểm tra nếu không tìm thấy tài khoản
            if (account == null)
            {
                return false;
            }

            // So sánh mật khẩu đã nhập với mật khẩu được mã hóa trong cơ sở dữ liệu
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(accountLogin.Password, account.Password);

            // Nếu mật khẩu không hợp lệ, trả về false
            if (!isPasswordValid)
            {
                return false;
            }

            // Nếu đúng mật khẩu, đăng nhập thành công
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
        public bool CheckEmployee(int employeeId)
        {
            var account = _context.Accounts.Where(p => p.EmployeeId == employeeId).FirstOrDefault();
            if (account == null)
            {
                return false;
            }
            return true;
        }
        public bool ChangePassword (int accountId, string newPassword)
        {
            var account = GetAccountById(accountId);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            account.Password = hashedPassword;
            _context.Update(account);
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
