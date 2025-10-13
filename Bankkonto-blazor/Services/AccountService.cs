namespace Bankkonto_blazor.Services;

public class AccountService : IAccountService
{
    // List of all accounts
    private readonly List<IBankAccount> _accounts = new();

    // Create new BankAccount, add to List of all accounts, and return
    public IBankAccount CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        return account;
    }

    // return list of accounts
    public List<IBankAccount> GetAccounts() => _accounts;

    // User input int determines return index from list _accounts
    public IBankAccount GetAccountIndex(int index) => _accounts[index];
}