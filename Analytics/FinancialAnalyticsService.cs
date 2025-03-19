using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analytics
{
    public class FinancialAnalyticsService : IFinancialAnalyticsService
    {
        private readonly IRepository<Operation> _operationRepository;

        public FinancialAnalyticsService(IRepository<Operation> operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public decimal GetIncomeExpenseDifference(DateTime startDate, DateTime endDate)
        {
            var operations = _operationRepository.GetAll()
                .Where(o => o.Date >= startDate && o.Date <= endDate);

            var totalIncome = operations
                .Where(o => o.Type == TransactionType.Income)
                .Sum(o => o.Amount);

            var totalExpense = operations
                .Where(o => o.Type == TransactionType.Expense)
                .Sum(o => o.Amount);

            return totalIncome - totalExpense;
        }

        public Dictionary<string, decimal> GroupOperationsByCategory(DateTime startDate, DateTime endDate)
        {
            return _operationRepository.GetAll()
                .Where(o => o.Date >= startDate && o.Date <= endDate)
                .GroupBy(o => o.CategoryId)
                .ToDictionary(
                    g => g.First().CategoryId.ToString(), 
                    g => g.Sum(o => o.Amount)
                );
        }
    }
}