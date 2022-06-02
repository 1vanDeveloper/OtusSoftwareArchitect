using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels.Interfaces;

public interface IRegisterViewModel
{
    Register Register { get; }
    
    public Task RegisterUser();
}