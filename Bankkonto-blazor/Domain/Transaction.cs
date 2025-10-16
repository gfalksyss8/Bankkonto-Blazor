using System.Net;

namespace Bankkonto_blazor.Domain;

public class TransactionBank : ITransaction
{
    // Constructor
    private IBankAccount SenderAccount { get; }
    private IBankAccount RecieverAccount { get; }
    public decimal Amount { get; private set; }
    public DateTime LastUpdated { get; private set; }
    IBankAccount ITransaction.SenderAccount => SenderAccount;
    IBankAccount ITransaction.RecieverAccount => RecieverAccount;

    // Constructor set
    public TransactionBank(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount)
    {
        SenderAccount = senderAccount;
        RecieverAccount = recieverAccount;
        Amount = amount;
        LastUpdated = DateTime.Now;
    }

    // Currency converter, currencies pegged to 1 SEK
    public Dictionary<string, decimal> ConversionRates = new Dictionary<string, decimal>
    {
        ["SEK"] = 1,
        ["USD"] = 10.5m,
        ["EUR"] = 11.0m,
        ["GBP"] = 13.0m,
        ["JPY"] = 0.075m
    };

    // Withdraws amount from sender, deposits amount*sender rate / reciever rate to reciever
    public void Transfer(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount)
    {
        senderAccount.Withdraw(amount);
        //recieverAccount.Deposit(amount);
        recieverAccount.Deposit(amount * CurrencyConverter(senderAccount) / CurrencyConverter(recieverAccount));
    }
    // Returns value of dictionary if currency matches key in ConversionRates dictionary, else throws exception
    public decimal CurrencyConverter(IBankAccount account)
    {
        foreach (var rates in ConversionRates)
        {
            if (account.Currency == rates.Key)
            {
                return rates.Value;
            }
        }
        throw new Exception("Currency not supported");
    }
}