namespace Bankkonto_blazor.Interfaces;

public interface ITransaction 
{
    IBankAccount SenderAccount { get; }
    IBankAccount RecieverAccount { get; }
    decimal Amount { get; }
    DateTime LastUpdated { get; }

    void Transfer(IBankAccount senderAccount, IBankAccount recieverAccount, decimal amount);
    
}