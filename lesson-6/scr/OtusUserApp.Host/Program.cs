using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using host = Microsoft.Extensions.Hosting.Host;

namespace OtusUserApp.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
