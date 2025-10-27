using System.Threading.Tasks;

namespace Bankkonto_blazor.Services;

public class AccountService : IAccountService
{
    // List of all accounts
    private const string StorageKey = "bankkonto-blazor.accounts";
    private readonly List<BankAccount> _accounts = new();
    private readonly IStorageService _storageService;
    private bool isLoaded;

    public AccountService(IStorageService storageService)
    {
        _storageService = storageService;
    }
    
    public async Task EnsureLoadedAsync() 
    {
        if(isLoaded) return;
        await IsInitialized();
    }

    private async Task IsInitialized()
    {
        var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);
        _accounts.Clear();
        if (fromStorage is { Count: > 0 }) { _accounts.AddRange(fromStorage); }
        isLoaded = true;
    }

    private async Task SaveAsync() => await _storageService.SetItemAsync(StorageKey, _accounts);

    // Create new BankAccount, add to List of all accounts, and return
    public async Task<BankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    public async Task RemoveAccount(int index)
    {
        _accounts.RemoveAt(index);
        await SaveAsync();
    }

    // return list of accounts
    public async Task<List<BankAccount>> GetAccounts()
    {
        return _accounts.Cast<BankAccount>().ToList();
    }

    // User input int determines return index from list _accounts
    public BankAccount GetAccountIndex(int index) => _accounts[index];

    public async Task Withdraw(BankAccount account, decimal withdrawAmount)
    {
        account.Withdraw(withdrawAmount);
        await SaveAsync();
    }

    public async Task Deposit(BankAccount account, decimal depositAmount)
    {
        account.Deposit(depositAmount);
        await SaveAsync();
    }
    public async Task Transfer(int senderIndex, int recieverIndex, decimal transferAmount)
    {
        var senderAccount = GetAccountIndex(senderIndex);
        var recieverAccount = GetAccountIndex(recieverIndex);
        senderAccount.Transfer(recieverAccount, transferAmount);
        await SaveAsync();
    }

}