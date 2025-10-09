namespace Bankkonto_blazor.Interfaces;

public interface IAccountService
{
    IBankAccount CreateAccount(string Name, string Currency, decimal initialBalance);
    List<IBankAccount> GetAccounts();
}