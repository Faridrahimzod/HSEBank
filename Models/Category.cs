using Models;
using System;

namespace Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public string Name { get; set; }

        public Category(TransactionType type, string name)
        {
            Id = Guid.NewGuid();
            Type = type;
            Name = name;
        }
    }
}