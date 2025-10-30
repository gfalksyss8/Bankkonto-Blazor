using System.Runtime;
using System.Text.Json.Serialization;

namespace Bankkonto_blazor.Domain;

public class BankAccount : IBankAccount
{
    // Constructor
    //private decimal initialBalance;
    public Guid Id { get; private set; } = Guid.NewGuid();
    public AccountType AccountType { get; private set; }
    public string Name { get; private set; } = "NoName";
    public string Currency { get; private set; } = "SEK";
    public decimal Balance { get;  private set; }
    public DateTime LastUpdated { get; private set; }
    public List<TransactionBank>? Transaction { get; set; } = new();
    public decimal InterestRate { get; private set; } = 0.0m;

    // Constructor set
    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
        if (AccountType == AccountType.Savings)
        {
            InterestRate = 0.02m; // 2% Interest
        }
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, string currency, decimal balance, DateTime lastUpdated, List<TransactionBank>? transaction)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = balance;
        LastUpdated = lastUpdated;
        Transaction = transaction ?? new();
        if (AccountType == AccountType.Savings)
        {
            InterestRate = 0.02m; // 2% Interest
        }
    }

    // Unassigned external account for deposits and withdrawals
    private static readonly BankAccount External = new BankAccount("External", AccountType.Deposit, "SEK", 0);

    public void Deposit(decimal amount)
    {
        Balance = decimal.Round(Balance += amount, 2);
        Transaction.Add(new TransactionBank(External, this, amount, TransactionType.Deposit));
    }

    public void Withdraw(decimal amount)
    {
        Balance = decimal.Round(Balance -= amount, 2);
        Transaction.Add(new TransactionBank(this, External, amount, TransactionType.Withdraw));
    }

    // Transfer methods
    public void Transfer(BankAccount recieverAccount, decimal amount)
    {
        Balance = decimal.Round(Balance -= amount, 2);
        Transaction.Add(new TransactionBank(this, recieverAccount, amount, TransactionType.TransferFrom));
        recieverAccount.Balance = decimal.Round(recieverAccount.Balance += amount * TransactionBank.CurrencyConverter(this) / TransactionBank.CurrencyConverter(recieverAccount), 2);
        recieverAccount.Transaction.Add(new TransactionBank(this, recieverAccount, amount, TransactionType.TransferTo));
    }

    public void DepositInterest()
    {
        if (AccountType == AccountType.Savings) {

            TimeSpan sinceCreation = DateTime.Now - LastUpdated;
            var yearly = sinceCreation.Days / 365;
            var interest = Balance * InterestRate * yearly;
            if (yearly > 0)
            {
                Balance = decimal.Round(Balance -= interest, 2);
                Transaction.Add(new TransactionBank(External, this, interest, TransactionType.Deposit));
            }
        }
    }
}