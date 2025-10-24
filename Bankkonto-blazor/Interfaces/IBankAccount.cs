namespace Bankkonto_blazor.Interfaces;
/// <summary>
/// Interface containing the BankAccount methods
/// </summary>
public interface IBankAccount
{
    Guid Id { get; }
    AccountType AccountType { get; }
    string Name { get; }
    string Currency { get; }
    decimal Balance { get; }
    DateTime LastUpdated { get; }
    List<TransactionBank> Transactions { get; }

    void Withdraw(decimal amount);
    void Deposit(decimal amount);
    void TransferTo(IBankAccount senderAccount, decimal amount);
    void Transfer(IBankAccount recieverAccount, decimal amount);
}