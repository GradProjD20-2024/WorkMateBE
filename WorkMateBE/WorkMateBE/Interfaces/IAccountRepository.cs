﻿using WorkMateBE.Dtos.AccountDto;
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
        bool CheckEmail(string email);
        bool Login(AccountLogin accountLogin);
        bool CheckEmployee(int employeeId);
    }
}
