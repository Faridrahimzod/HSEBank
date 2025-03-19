using System;

namespace Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public BankAccount(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = 0m;
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
        }

        public void SetBalance(decimal newBalance)
        {
            Balance = newBalance;
        }
    }
}