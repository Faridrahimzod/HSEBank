using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<BankAccount> Accounts { get; }
        IRepository<Category> Categories { get; }
        IRepository<Operation> Operations { get; }
        IFinancialAnalyticsService Analytics { get; } 

        void Commit();
    }
}
