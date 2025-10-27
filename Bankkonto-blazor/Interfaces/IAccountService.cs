namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task EnsureLoadedAsync();
    Task<BankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task RemoveAccount(int index);
    Task<List<BankAccount>> GetAccounts();
    BankAccount GetAccountIndex(int index);

    Task Transfer(int senderIndex, int recieverIndex, decimal transferAmount);

    Task Deposit(BankAccount account, decimal depositAmount);
    Task Withdraw(BankAccount account, decimal withdrawAmount);
}