using IConstruye.Factura.Infrastructure.Data;

namespace IConstruye.Factura
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await CreateAndMigrateDb(host);
            await host.RunAsync();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static async Task CreateAndMigrateDb(IHost host, int retry = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var moviesContext = services.GetRequiredService<InvoiceContext>();
                    await InvoiceContextSeed.SeedAsync(moviesContext,  loggerFactory);
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError($"Exception occured in API {e.Message}");
                }
            }
        }
    }
}