using System;

namespace Interfaces
{
    public interface IExecutionTimer
    {
        TimeSpan Measure(Action action);
        T Measure<T>(Func<T> func);
    }
}