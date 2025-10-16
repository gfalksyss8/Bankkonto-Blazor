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
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }

    // Constructor set
    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
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

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        Balance -= amount;
    }
    
}