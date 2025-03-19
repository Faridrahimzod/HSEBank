using System;
using System.Diagnostics;
using Interfaces;

namespace Statistics
{
    public class ExecutionTimer : IExecutionTimer
    {
        public TimeSpan Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.Elapsed;
        }

        public T Measure<T>(Func<T> func)
        {
            var sw = Stopwatch.StartNew();
            var result = func();
            sw.Stop();
            return result;
        }
    }
}