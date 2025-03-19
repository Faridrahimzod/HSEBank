// Core/Repositories/AccountRepository.cs
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class AccountRepository : IRepository<BankAccount>
    {
        private readonly List<BankAccount> _accounts = new();

        public void Add(BankAccount account) => _accounts.Add(account);

        public void Update(BankAccount account)
        {
            var index = _accounts.FindIndex(a => a.Id == account.Id);
            if (index != -1) _accounts[index] = account;
        }

        public void Delete(Guid id) => _accounts.RemoveAll(a => a.Id == id);

        public BankAccount GetById(Guid id) => _accounts.FirstOrDefault(a => a.Id == id);

        public IEnumerable<BankAccount> GetAll() => _accounts.AsReadOnly();
    }
}