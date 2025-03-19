// Core/Repositories/OperationRepository.cs
using Interfaces;
using Models;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class OperationRepository : IRepository<Operation>
    {
        private readonly List<Operation> _operations = new();

        public void Add(Operation operation) => _operations.Add(operation);

        public void Update(Operation operation)
        {
            var index = _operations.FindIndex(o => o.Id == operation.Id);
            if (index != -1) _operations[index] = operation;
        }

        public void Delete(Guid id) => _operations.RemoveAll(o => o.Id == id);

        public Operation GetById(Guid id) => _operations.FirstOrDefault(o => o.Id == id);

        public IEnumerable<Operation> GetAll() => _operations.AsReadOnly();
    }
}