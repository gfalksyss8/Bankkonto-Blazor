namespace Bankkonto_blazor.Interfaces;

public interface ITransaction 
{
    IBankAccount SenderAccount { get; }
    IBankAccount RecieverAccount { get; }
    decimal Amount { get; }
    decimal CurrencyModifier { get; }
    DateTime LastUpdated { get; }
    decimal ToSek => Amount * CurrencyModifier;

    void Transfer(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount);
    
}