// Core/Services/Analytics/FinancialAnalyticsService.cs

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

        // a. Разница доходов и расходов
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

        // b. Группировка по категориям (базовая реализация)
        public Dictionary<string, decimal> GroupOperationsByCategory(DateTime startDate, DateTime endDate)
        {
            return _operationRepository.GetAll()
                .Where(o => o.Date >= startDate && o.Date <= endDate)
                .GroupBy(o => o.CategoryId)
                .ToDictionary(
                    g => g.First().CategoryId.ToString(), // Замените на имя категории через репозиторий
                    g => g.Sum(o => o.Amount)
                );
        }
    }
}