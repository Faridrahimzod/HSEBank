// Core/Repositories/CategoryRepository.cs
using Interfaces;
using Models;
using Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly List<Category> _categories = new();

        public void Add(Category category) => _categories.Add(category);

        public void Update(Category category)
        {
            var index = _categories.FindIndex(c => c.Id == category.Id);
            if (index != -1) _categories[index] = category;
        }

        public void Delete(Guid id) => _categories.RemoveAll(c => c.Id == id);

        public Category GetById(Guid id) => _categories.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Category> GetAll() => _categories.AsReadOnly();
    }
}