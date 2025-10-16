namespace Bankkonto_blazor.Services;

public class AccountService : IAccountService
{
    // List of all accounts
    private const string StorageKey = "bankkonto-blazor.accounts";
    private readonly List<BankAccount> _accounts = new();
    private readonly IStorageService _storageService;
    public AccountService(IStorageService storageService) => _storageService = storageService;

    private bool isLoaded;
    private async Task IsInitialized()
    {
        if (!isLoaded)
        {
            var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);
            _accounts.Clear();
            if (fromStorage is { Count: > 0 }) { _accounts.AddRange(fromStorage); }
            isLoaded = true;
        }
    }

    private async Task SaveAsync() => await _storageService.SetItemAsync(StorageKey, _accounts);

    // Create new BankAccount, add to List of all accounts, and return
    public async Task<IBankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        await IsInitialized();
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    public async void RemoveAccount (int index)
    {
        await IsInitialized();
        _accounts.RemoveAt(index);
        await SaveAsync();
    }

    // return list of accounts
    public async Task<List<IBankAccount>> GetAccounts()
    {
        await IsInitialized();
        return _accounts.Cast<IBankAccount>().ToList();
    }

    // User input int determines return index from list _accounts
    public IBankAccount GetAccountIndex(int index) => _accounts[index];
}