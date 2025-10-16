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
    public decimal CurrencyModifier { get; private set; }
    public decimal ToSek => Amount * CurrencyModifier;

    // Constructor set
    public TransactionBank(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount)
    {
        SenderAccount = senderAccount;
        RecieverAccount = recieverAccount;
        Amount = amount;
        LastUpdated = DateTime.Now;
    }

    // Currency converter
    public Dictionary<string, decimal> ConversionRates = new Dictionary<string, decimal>
    {
        ["sek"] = 1,
        ["usd"] = 10.5m,
        ["eur"] = 11.0m,
        ["gbp"] = 13.0m,
        ["jpy"] = 0.075m
    };

    // Transfer method
    public void Transfer(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount)
    {
        senderAccount.Withdraw(amount);
        recieverAccount.Deposit(amount);
        //recieverAccount.Deposit(amount * CurrencyConverter(senderAccount) / CurrencyConverter(recieverAccount));
    }
    public decimal CurrencyConverter(IBankAccount account)
    {
        foreach (var currency in ConversionRates)
        {
            if (account.Currency.ToLower() == currency.Key.ToLower())
            {
                return currency.Value;
            }
            else
            {
                throw new Exception("Currency not supported");
            }
        }
        throw new Exception("Currency not supported");
    }
}