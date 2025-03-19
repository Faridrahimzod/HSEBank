using Interfaces;
using System;
using System.Collections.Generic;

namespace Statistics
{
    public class StatisticsAggregator
    {
        private readonly Dictionary<string, List<TimeSpan>> _metrics = new();

        public void Record(string scenarioName, TimeSpan elapsed)
        {
            if (!_metrics.ContainsKey(scenarioName))
                _metrics[scenarioName] = new List<TimeSpan>();

            _metrics[scenarioName].Add(elapsed);
        }

        public void PrintReport()
        {
            foreach (var (scenario, times) in _metrics)
            {
                var avg = TimeSpan.FromMilliseconds(
                    times.Select(t => t.TotalMilliseconds).Average()
                );
                Console.WriteLine($"{scenario}: Avg {avg:mm\\:ss\\.fff}, Total {times.Count}");
            }
        }
        public void Track(string scenario, Action action, IExecutionTimer timer)
        {
            var elapsed = timer.Measure(action);
            Record(scenario, elapsed);
        }
    }
}