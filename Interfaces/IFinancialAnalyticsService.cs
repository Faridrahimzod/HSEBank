using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IFinancialAnalyticsService
    {
        decimal GetIncomeExpenseDifference(DateTime startDate, DateTime endDate);
        Dictionary<string, decimal> GroupOperationsByCategory(DateTime startDate, DateTime endDate);
    }
}