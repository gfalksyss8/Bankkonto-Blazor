namespace Bankkonto_blazor.Domain;

public class BankAccount : IBankAccount
{
    // Constructor
    private decimal initialBalance;
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

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        Balance -= amount;
    }

    public void Transaction(decimal amount, string reciever, List<IBankAccount> accounts)
    {
        foreach (var account in accounts)
        {
            if (reciever == account.Name)
            {
                //account.Balance += amount;
                Balance -= amount;
            }
        }
    }
    
}