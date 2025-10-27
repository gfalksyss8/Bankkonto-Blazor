using System.Net;
using System.Runtime;
using System.Text.Json.Serialization;

namespace Bankkonto_blazor.Domain;

public class TransactionBank
{
    // Constructor
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string SenderAccountName { get; }
    public Guid SenderAccountId { get; }
    public string RecieverAccountName { get; }
    public Guid RecieverAccountId { get; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; } 
    public TransactionType TransactionType { get; private set; }
    public DateTime LastUpdated { get; private set; }

    // Constructor sets
    [JsonConstructor]
    public TransactionBank(Guid id, string senderAccountName, Guid senderAccountId, string recieverAccountName, Guid recieverAccountId, decimal amount, decimal balanceAfter, TransactionType transactionType, DateTime lastUpdated)
    {
        Id = id;
        SenderAccountName = senderAccountName;
        SenderAccountId = senderAccountId;
        RecieverAccountName = recieverAccountName;
        RecieverAccountId = recieverAccountId;
        Amount = amount;
        BalanceAfter = balanceAfter;
        TransactionType = transactionType;
        LastUpdated = lastUpdated;
    }
    // Runtime constructor
    public TransactionBank(BankAccount senderAccount, BankAccount recieverAccount, decimal amount, TransactionType transactionType)
    {
        SenderAccountName = senderAccount.Name;
        SenderAccountId = senderAccount.Id;
        RecieverAccountName = recieverAccount.Name;
        RecieverAccountId = recieverAccount.Id;
        Amount = amount;
        TransactionType = transactionType;
        LastUpdated = DateTime.Now;
        /*
        BalanceAfter = (this.TransactionType == TransactionType.TransferTo)
            ? senderAccount.Balance - Amount
            : senderAccount.Balance + Amount;
        */
    }

    // Currency converter, currencies pegged to 1 SEK
    public static Dictionary<string, decimal> ConversionRates = new Dictionary<string, decimal>
    {
        ["SEK"] = 1,
        ["USD"] = 10.5m,
        ["EUR"] = 11.0m,
        ["GBP"] = 13.0m,
        ["JPY"] = 0.075m
    };

    // Returns value of dictionary if currency matches key in ConversionRates dictionary, else throws exception
    public static decimal CurrencyConverter(BankAccount account)
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