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
    public readonly List<TransactionBank> _transactions = new();
    public List<TransactionBank> Transactions => _transactions;

    // Constructor set
    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance, List<TransactionBank>? transactions)
    {
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, string currency, decimal balance, DateTime lastUpdated)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = balance;
        LastUpdated = lastUpdated;
    }

    // Unassigned external account for deposits and withdrawals
    private static readonly BankAccount External = new BankAccount("External", AccountType.Deposit, "SEK", 0, new List<TransactionBank>());

    public void Deposit(decimal amount)
    {
        Balance = decimal.Round(Balance += amount, 2);
        _transactions.Add(new TransactionBank(External, this, amount, TransactionType.Deposit));
    }

    public void Withdraw(decimal amount)
    {
        Balance = decimal.Round(Balance -= amount, 2);
        _transactions.Add(new TransactionBank(this, External, amount, TransactionType.Withdraw));
    }

    // Transfer methods
    public void Transfer(IBankAccount recieverAccount, decimal amount)
    {
        Balance = decimal.Round(Balance -= amount, 2);
        _transactions.Add(new TransactionBank(this, recieverAccount, amount, TransactionType.TransferFrom));
        recieverAccount.TransferTo(this, amount);
    }
    public void TransferTo(IBankAccount senderAccount, decimal amount)
    {
        Balance = decimal.Round(Balance += amount * TransactionBank.CurrencyConverter(senderAccount) / TransactionBank.CurrencyConverter(this), 2);
        _transactions.Add(new TransactionBank(senderAccount, this, amount, TransactionType.TransferTo));
        }
}