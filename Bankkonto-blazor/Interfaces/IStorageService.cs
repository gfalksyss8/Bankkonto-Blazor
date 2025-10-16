namespace Bankkonto_blazor.Interfaces;

public interface IStorageService
{
    //Save
    Task SetItemAsync<T>(string key, T value);
    // Hämta
    Task<T> GetItemAsync<T>(string key);
}