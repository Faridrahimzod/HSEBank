using Interfaces;
using Models;
using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IGroupingStrategy
    {
        Dictionary<string, decimal> Group(IEnumerable<Operation> operations, IRepository<Category> categoryRepository);
    }
}