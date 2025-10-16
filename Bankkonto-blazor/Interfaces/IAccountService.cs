namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task<IBankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    IBankAccount GetAccountIndex(int index);
}