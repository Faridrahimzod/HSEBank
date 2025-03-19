using Interfaces;
using Models;
using System;

namespace Analytics
{
    public class BalanceRecalculationService
    {
        private readonly IRepository<BankAccount> _accountRepository;
        private readonly IRepository<Operation> _operationRepository;

        public BalanceRecalculationService(
            IRepository<BankAccount> accountRepository,
            IRepository<Operation> operationRepository)
        {
            _accountRepository = accountRepository;
            _operationRepository = operationRepository;
        }

        // Пересчет баланса для всех счетов
        public void RecalculateAllBalances()
        {
            foreach (var account in _accountRepository.GetAll())
            {
                RecalculateBalance(account.Id);
            }
        }

        // Пересчет баланса для конкретного счета
        public void RecalculateBalance(Guid accountId)
        {
            var account = _accountRepository.GetById(accountId);
            if (account == null) return;

            var operations = _operationRepository.GetAll()
                .Where(o => o.BankAccountId == accountId);

            decimal newBalance = operations
                .Sum(o => o.Type == TransactionType.Income ? o.Amount : -o.Amount);

            account.UpdateBalance(newBalance - account.Balance); // Корректировка разницы
            _accountRepository.Update(account);
        }
    }
}