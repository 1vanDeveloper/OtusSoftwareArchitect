using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels.Interfaces;

public interface IAccountViewModel
{
    Account Account { get; }
    
    public Task GetAsync();

    public Task UpdateAsync();
}