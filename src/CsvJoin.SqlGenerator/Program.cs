using CsvJoin.SqlGenerator.Services;
using CsvJoin.SqlGenerator.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CsvJoin.SqlGenerator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetRequiredService<Application>()
                .RunAsync(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            AddServices(services);
            AddApplication(services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services
                .AddTransient<ISqlPreparator, SqlPreparator>()
                .AddTransient<ISqlFormatter, SqlFormatter>()
                .AddTransient<ISqlSaver, SqlSaver>();
        }

        private static void AddApplication(IServiceCollection services)
        {
            services.AddTransient<Application>();
        }
    }
}
