using System;

namespace Analytics
{
    public class SpendingForecastService
    {
        public decimal ForecastMonthlyExpenses(decimal currentSpending, int months)
        {
            // Простейший пример: линейный прогноз
            return currentSpending * months;
        }
    }
}