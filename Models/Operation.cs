using System;

namespace Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public Guid BankAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }

        public Operation(
            TransactionType type,
            Guid bankAccountId,
            decimal amount,
            DateTime date,
            Guid categoryId,
            string? description = null)
        {
            Id = Guid.NewGuid();
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            CategoryId = categoryId;
            Description = description;
        }
    }
}