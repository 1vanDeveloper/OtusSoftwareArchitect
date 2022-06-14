using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels.Interfaces;

public interface IAccountViewModel
{
    Account Account { get; }
    
    Money Money { get; }

    public Task InitAsync();

    public Task UpdateAccountAsync();

    public Task PutMoneyAsync();
}