namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task<IBankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task RemoveAccount(int index);
    Task<List<IBankAccount>> GetAccounts();
    IBankAccount GetAccountIndex(int index);

    Task Transfer(int senderIndex, int recieverIndex, decimal transferAmount);

    Task Deposit(IBankAccount account, decimal depositAmount);
    Task Withdraw(IBankAccount account, decimal withdrawAmount);
}