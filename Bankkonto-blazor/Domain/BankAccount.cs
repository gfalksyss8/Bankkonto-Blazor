namespace Bankkonto_blazor.Domain;

public class BankAccount : IBankAccount
{
    private decimal initialBalance;

    public BankAccount(string name, string currency, decimal initialBalance)
    {
        Name = name;
        Currency = currency;
        this.initialBalance = initialBalance;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = "NoName";

    public string Currency { get; private set; } = "SEK";

    public decimal Balance { get; private set; }

    public DateTime LastUpdated { get; private set; }

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
                account.Balance += amount;
                Balance -= amount;
            }
        }
    }
    
}