using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.JSInterop;

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
    }

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
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException("JSON empty");
        }

        await _storageService.ImportFromJsonAsync<List<BankAccount>>(StorageKey, json);
        await EnsureLoadedAsync();
    }

    // Password authorization
    public event Action OnAuthStateChanged;
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
    public void NotifyAuthStateChanged() => OnAuthStateChanged?.Invoke();
    public bool IsAuthorized()
    {
        return Authorized;
    }

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
    public async Task InterestUpdater()
    {
        var _accounts = await GetAccounts();
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