namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task EnsureLoadedAsync();
    Task ExportAccountsAsync();
    Task ImportAccountsAsync(string json);
    event Action OnAccountsChanged;

    bool TryAuthorize(string password);
    bool IsAuthorized();
    event Action OnAuthStateChanged;
    void NotifyAuthStateChanged();
    string GetPassword();
    Task SetPassword(string newPassword);

    Task<BankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task RemoveAccount(BankAccount account);
    List<BankAccount> GetAccounts();
    BankAccount GetAccountIndex(int index);

    Task Transfer(int senderIndex, int recieverIndex, decimal transferAmount);
    Task Deposit(BankAccount account, decimal depositAmount);
    Task Withdraw(BankAccount account, decimal withdrawAmount);

    Task DevAddInterest(BankAccount account);
    Task DevDepositInterest(BankAccount account);
}