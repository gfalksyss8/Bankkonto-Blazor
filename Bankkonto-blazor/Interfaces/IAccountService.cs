namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    Task EnsureLoadedAsync();
    bool TryAuthorize(string password);
    bool IsAuthorized();
    event Action OnAuthStateChanged;
    void NotifyAuthStateChanged();
    string GetPassword();
    Task SetPassword(string newPassword);
    Task<BankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance);
    Task RemoveAccount(int index);
    Task<List<BankAccount>> GetAccounts();
    BankAccount GetAccountIndex(int index);

    Task Transfer(int senderIndex, int recieverIndex, decimal transferAmount);

    Task Deposit(BankAccount account, decimal depositAmount);
    Task Withdraw(BankAccount account, decimal withdrawAmount);
}