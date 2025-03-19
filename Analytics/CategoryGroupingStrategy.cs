using Analytics;
using Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Analytics
{
    public class CategoryGroupingStrategy : IGroupingStrategy
    {
        public Dictionary<string, decimal> Group(
            IEnumerable<Operation> operations,
            IRepository<Category> categoryRepository)
        {
            return operations
                .GroupBy(o => o.CategoryId)
                .ToDictionary(
                    g => categoryRepository.GetById(g.Key)?.Name ?? "Unknown",
                    g => g.Sum(o => o.Amount)
                );
        }
    }
}