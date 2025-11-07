namespace Bankkonto_blazor.Services;

public class AccountService : IAccountService
{
    // Fields
    private const string StorageKey = "bankkonto-blazor.accounts";
    private const string PassKey = "bankkonto-blazor.password";
    private readonly List<BankAccount> _accounts = new();
    private readonly IStorageService _storageService;
    private bool isLoaded;
    private string _password = "admin";
    public bool Authorized { get; private set; }

    public AccountService(IStorageService storageService)
    {
        _storageService = storageService;
    }
    
    // Loading from storage to password and accounts
    public async Task EnsureLoadedAsync() 
    {
        if(isLoaded) return;
        await IsInitialized();
    }

    private async Task IsInitialized()
    {
        var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);
        string fromStoragePass = await _storageService.GetItemAsync<string>(PassKey);
        _accounts.Clear();

        if (fromStorage is { Count: > 0 }) { _accounts.AddRange(fromStorage); }
        if (!string.IsNullOrWhiteSpace(fromStoragePass)) { _password = fromStoragePass; }
        else { await SaveAsyncPassword(); }
            
        isLoaded = true;
        await InterestUpdater();
        System.Console.WriteLine("IsInitialized successfully called");
    }

    // Save into storage, call after any change
    private async Task SaveAsync() => await _storageService.SetItemAsync(StorageKey, _accounts);
    private async Task SaveAsyncPassword() => await _storageService.SetItemAsync(PassKey, _password);

    // Import & Export methods, download trigger
    public async Task ExportAccountsAsync()
    {
        string json = await _storageService.ExportAsJsonAsync<List<BankAccount>>(StorageKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            json = "[]";
        }
        await _storageService.DownloadAsJsonAsync("accounts-export.json", json);
    }

    public async Task ImportAccountsAsync(string json)
    {
        System.Console.WriteLine("Import method called");
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("but import string was empty");
        }

        var import = await _storageService.ImportFromJsonAsync<List<BankAccount>>(StorageKey, json);
        _accounts.Clear();

        if (import is { Count: > 0 }) { _accounts.AddRange(import); }
        System.Console.WriteLine("Accounts imported to localStorage");

        NotifyAccountsChanged();
        await InterestUpdater();
    }

    // Update account data after import
    public event Action OnAccountsChanged;
    public void NotifyAccountsChanged()
    {
        System.Console.WriteLine("OnAccountsChanged invoked");
        OnAccountsChanged?.Invoke();
    }

    // Password authorization
    public bool TryAuthorize(string password)
    {
        if (Authorized == false)
        {
            if (password == _password)
            {
                Authorized = true;
                NotifyAuthStateChanged();
                return Authorized;
            }
            return Authorized;
        }
        else
        {
            return Authorized;
        }
    }
    public bool IsAuthorized()
    {
        return Authorized;
    }

    // State change to update NavMenu from Home
    public event Action OnAuthStateChanged;
    public void NotifyAuthStateChanged() => OnAuthStateChanged?.Invoke();

    // Get & set password methods
    public string GetPassword()
    {
        return _password;
    }
    public async Task SetPassword(string newPassword)
    {
        _password = newPassword;
        await SaveAsyncPassword();
    }

    // Create new BankAccount, add to List of all accounts, and return
    public async Task<BankAccount> CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    // Remove chosen account
    public async Task RemoveAccount(int index)
    {
        _accounts.RemoveAt(index);
        await SaveAsync();
    }

    // return list of accounts
    public List<BankAccount> GetAccounts()
    {
        return _accounts;
    }

    // User input int determines return index from list _accounts
    public BankAccount GetAccountIndex(int index) => _accounts[index];

    // Methods that calls BankAccount logic to change balance for account
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

    // Calls both interest methods for all accounts, 
    // method adds depending on DateTime, so can be called anytime, 
    // but should only be called on initialization and import
    public async Task InterestUpdater()
    {
        var _accounts = GetAccounts();
        foreach (var account in _accounts)
        {
            account.AddInterest();
            account.DepositInterest();
        }
        await SaveAsync();
    }

    // DEV
    public async Task DevAddInterest(BankAccount account)
    {
        account.DevAddInterest();
        account.AddInterest();
        await SaveAsync();
    }
    public async Task DevDepositInterest(BankAccount account)
    {
        account.DevDepositInterest();
        await SaveAsync();
    }
}