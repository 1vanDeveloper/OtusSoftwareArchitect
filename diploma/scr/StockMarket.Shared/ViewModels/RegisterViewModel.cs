using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

[DataContract]
public class RegisterViewModel: IRegisterViewModel
{
    public Register Register { get; } = new ();
    
    private readonly HttpClient _httpClient;

    public RegisterViewModel()
    {
    }
    public RegisterViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task RegisterUser()
    {
        const string url = "api/register";

        var response = await _httpClient.PostAsync<Register>(url, Register);
        if (response.Code != HttpStatusCode.OK)
        {
            throw new Exception("Register failed!");
        }
    }
}