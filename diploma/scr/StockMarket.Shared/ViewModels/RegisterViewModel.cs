using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

[DataContract]
public class RegisterViewModel: IRegisterViewModel
{
    public Register Register { get; } = new ();
    
    private readonly HttpClient _httpClient;
    private readonly IToastService _toastService;

    public RegisterViewModel(HttpClient httpClient, IToastService toastService)
    {
        _httpClient = httpClient;
        _toastService = toastService;
    }
    
    public async Task RegisterUser()
    {
        const string url = "api/register";

        var response = await _httpClient.PostAsync<Register>(url, Register);
        
        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
        }
        else
        {
            _toastService.ShowSuccess("Register success!");
        }
    }
}