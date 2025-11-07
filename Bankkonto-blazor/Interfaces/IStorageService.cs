namespace Bankkonto_blazor.Interfaces;

public interface IStorageService
{
    // Set
    Task SetItemAsync<T>(string key, T value);
    // Get
    Task<T> GetItemAsync<T>(string key);

    // Import and export
    Task<string> ExportAsJsonAsync<T>(string key);
    Task<T> ImportFromJsonAsync<T>(string key, string json);
    Task DownloadAsJsonAsync(string fileName, string json);
}