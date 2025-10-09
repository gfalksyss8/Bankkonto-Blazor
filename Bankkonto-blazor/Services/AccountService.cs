using Bankkonto_blazor.Domain;

namespace Bankkonto_blazor.Services;

public class AccountService : IAccountService
{
    // List of all accounts
    List<IBankAccount> accounts = new List<IBankAccount>();

    // Create new BankAccount using ID, add to List<IBankAccount> accounts
    public IBankAccount CreateAccount(string name, string currency, decimal initialBalance)
    {
        BankAccount Id = new BankAccount(name, currency, initialBalance);
        accounts.Add(Id);
        return Id;
    }

    // return list of accounts
    public List<IBankAccount> GetAccounts()
    {
        return accounts;
    }
}