using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using StockMarket.Shared.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StockMarket.Shared.Extensions;

/// <summary>
/// Сахар на запросы с аутентификацией
/// </summary>
public static class HttpClientExtensions
{
    public static Task<(HttpStatusCode Code, T Response, Error Error)> GetAsync<T>(this HttpClient httpClient,
        string url,
        string token = null)
    {
        return httpClient.SendAsync<T>(HttpMethod.Get, url, token: token);
    }

    public static Task<(HttpStatusCode Code, T Response, Error Error)> PostAsync<T>(this HttpClient httpClient,
        string url,
        object data,
        string token = null)
    {
        var content = new StringContent(JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json");

        return httpClient.PostAsync<T>(url, content, token);
    }

    public static Task<(HttpStatusCode Code, T Response, Error Error)> PostAsync<T>(this HttpClient httpClient,
        string url,
        HttpContent content,
        string token = null)
    {
        return httpClient.SendAsync<T>(HttpMethod.Post, url, content, token);
    }

    public static Task<(HttpStatusCode Code, Error Error)> PutAsync(this HttpClient httpClient,
        string url,
        object data,
        string token = null)
    {
        var content = new StringContent(JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json");

        return httpClient.SendAsync(HttpMethod.Put, url, content, token);
    }

    private static async Task<(HttpStatusCode Code, T Response, Error Error)> SendAsync<T>(this HttpClient httpClient, 
        HttpMethod method, 
        string url, 
        HttpContent content = null, 
        string token = null)
    {
        using var request = new HttpRequestMessage(method, url);
        request.SetBrowserRequestMode(BrowserRequestMode.Cors);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (content is not null)
        {
            request.Content = content;
        }

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return (response.StatusCode, default,  JsonSerializer.Deserialize<Error>(responseBody, jsonSerializerOptions));
        }
        
        return (response.StatusCode, JsonSerializer.Deserialize<T>(responseBody, jsonSerializerOptions), default);
    }
    
    
    private static async Task<(HttpStatusCode Code, Error Error)> SendAsync(this HttpClient httpClient, 
        HttpMethod method, 
        string url, 
        HttpContent content = null, 
        string token = null)
    {
        using var request = new HttpRequestMessage(method, url);
        request.SetBrowserRequestMode(BrowserRequestMode.Cors);

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        if (content is not null)
        {
            request.Content = content;
        }

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return (response.StatusCode, JsonSerializer.Deserialize<Error>(responseBody, jsonSerializerOptions));
        }
        
        return (response.StatusCode, default);
    }
 }