// IARF STRESS (csharp, hard/tricky): auto_property_private_set
// Auto-implemented properties with asymmetric accessibility (public get, private/protected set) create
// an implicit compiler-generated backing field with no explicit declaration site -- the semantic graph
// must synthesize that field node rather than expecting a textual `private readonly` declaration.

using System;

namespace Fixture03
{
    public class BankAccount
    {
        public string Owner { get; }
        public decimal Balance { get; private set; }
        public DateTime OpenedAt { get; init; } = DateTime.UtcNow;
        protected internal int TransactionCount { get; protected set; }

        public BankAccount(string owner, decimal openingBalance)
        {
            Owner = owner;
            Balance = openingBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Balance += amount;
            TransactionCount++;
        }

        public bool TryWithdraw(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount;
            TransactionCount++;
            return true;
        }
    }

    public class Program
    {
        public static void Main()
        {
            var acct = new BankAccount("Alice", 100m);
            acct.Deposit(50m);
            acct.TryWithdraw(30m);
            Console.WriteLine($"{acct.Owner}: {acct.Balance} ({acct.TransactionCount} txns)");
        }
    }
}
