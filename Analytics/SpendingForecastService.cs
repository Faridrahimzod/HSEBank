using System;

namespace Analytics
{
    public class SpendingForecastService
    {
        public decimal ForecastMonthlyExpenses(decimal currentSpending, int months)
        {
            
            return currentSpending * months;
        }
    }
}