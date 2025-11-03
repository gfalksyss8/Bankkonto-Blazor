
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Bankkonto_blazor.Services;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public StorageService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T> GetItemAsync<T>(string key)
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrWhiteSpace(json))
                return default;
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
        catch (Exception exception)
        {
            System.Console.WriteLine(exception);
            throw;
        }
    }
}