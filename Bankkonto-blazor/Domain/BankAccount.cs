using System.Runtime;
using System.Text.Json.Serialization;

namespace Bankkonto_blazor.Domain;

public class BankAccount : IBankAccount
{
    // Constructor
    public Guid Id { get; private set; } = Guid.NewGuid();
    public AccountType AccountType { get; private set; }
    public string Name { get; private set; } = "NoName";
    public string Currency { get; private set; } = "SEK";
    public decimal Balance { get;  private set; }
    public DateTime LastUpdated { get; private set; }
    public List<TransactionBank>? Transaction { get; set; } = new();
    public decimal PendingInterest { get; private set; }
    public DateTime InterestDate { get; private set; }
    public DateTime DepositInterestCountdown { get; private set; }

    // Constructor set
    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
        InterestDate = DateTime.Now;
        DepositInterestCountdown = DateTime.Now;
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, string currency, decimal balance, DateTime lastUpdated, List<TransactionBank>? transaction, decimal pendingInterest, DateTime interestDate, DateTime depositInterestCountdown)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = balance;
        LastUpdated = lastUpdated;
        Transaction = transaction ?? new();
        PendingInterest = pendingInterest;
        InterestDate = interestDate;
        DepositInterestCountdown = depositInterestCountdown;

    }

    // Unassigned external account for deposits and withdrawals
    private static readonly BankAccount External = new BankAccount("External", AccountType.Deposit, "SEK", 0);

    // Interestt rate, 2%
    decimal InterestRate = 0.02m;

    // Deposit and withdrawal methods, adds transaction to Transaction list for each bank account
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

    // Adds value of (Balance * 0,02 * Days since this method was called/365) to decimal PendingInterest. Should be run through AccountService.InterestUpdater() at startup (Initialization)
    public void AddInterest()
    {
        if (AccountType == AccountType.Savings)
        {
            TimeSpan sinceLastAddition = DateTime.Now - InterestDate;
            decimal yearly = sinceLastAddition.Days / 365m;
            PendingInterest += Balance * InterestRate * yearly;
            InterestDate = DateTime.Now;
        }
    }
    // Deposits pending interest if a year or more has passed, removes 1 year of days from DepositInterstCountdown
    public void DepositInterest()
    {
        if (AccountType == AccountType.Savings)
        {
            TimeSpan lastInterestDeposit = DateTime.Now - DepositInterestCountdown;
            if (lastInterestDeposit.Days > 365)
            {
                Balance += PendingInterest;
                Transaction.Add(new TransactionBank(External, this, PendingInterest, TransactionType.Interest));
                PendingInterest = 0;
                DepositInterestCountdown = DepositInterestCountdown.AddDays(-365);
            }
        }
    }

    // Dev commands for testing interest function, simulates 10 days passed by removing 10 days from InterestDate
    public void DevAddInterest()
    {
        InterestDate = InterestDate.AddDays(-10);
    }

    // Immediately deposits pending interest to balance
    public void DevDepositInterest()
    {
        Balance = decimal.Round(Balance += PendingInterest, 2);
        Transaction.Add(new TransactionBank(External, this, PendingInterest, TransactionType.Interest));
        PendingInterest = 0;
        // (Dev button doesn't reset interest cooldown timer)
        //DepositInterestCountdown = DepositInterestCountdown.AddDays(-365);
    }
}