namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task<IBankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    void RemoveAccount(int index);
    Task<List<IBankAccount>> GetAccounts();
    IBankAccount GetAccountIndex(int index);
}