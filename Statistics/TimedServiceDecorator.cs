using Interfaces;
using Models;
using System;
using System.Diagnostics;

namespace Statistics
{
    // 1. Добавляем интерфейс для финансовых сервисов
    public interface IFinancialService
    {
        void CreateAccount(string name);
        void AddOperation(Operation operation);
    }

    public class TimedServiceDecorator : IFinancialService
    {
        private readonly IFinancialService _decoratedService;
        private readonly IExecutionTimer _timer;
        private readonly StatisticsAggregator _stats;

        public TimedServiceDecorator(
            IFinancialService service,
            IExecutionTimer timer,
            StatisticsAggregator stats)
        {
            _decoratedService = service;
            _timer = timer;
            _stats = stats;
        }

        public void CreateAccount(string name)
        {
            // 2. Сохраняем результат замера
            var elapsed = _timer.Measure(() => _decoratedService.CreateAccount(name));
            // 3. Передаем оба параметра
            _stats.Record("CreateAccount", elapsed);
        }

        public void AddOperation(Operation operation)
        {
            var elapsed = _timer.Measure(() => _decoratedService.AddOperation(operation));
            _stats.Record("AddOperation", elapsed);
        }
    }
}