using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using StockMarket.Components;
using StockMarket.Shared.Models;
using StockMarket.Shared.Extensions;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

#region ConfigureServices

// setting client host environment 
builder.Services.AddSingleton<IHostingEnvironment>(
    new HostingEnvironment { EnvironmentName = builder.HostEnvironment.Environment });

// adding client app settings 
var applicationSettingsSection = builder.Configuration.GetSection("ApplicationSettings");
builder.Services.Configure<ApplicationSettings>(options =>
{
    applicationSettingsSection.Bind(options);
});

// adding application services
builder.Services.AddStockMarketServices(applicationSettingsSection.Get<ApplicationSettings>());

#endregion

await builder.Build().RunAsync();