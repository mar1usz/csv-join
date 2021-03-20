using CsvJoin.Services;
using CsvJoin.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CsvJoin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetRequiredService<Application>().RunAsync(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISqlPreparator, DefaultSqlPreparator>();
            services.AddTransient<ISqlFormatter, DefaultSqlFormatter>();
            services.AddTransient<ISqlExecutor, DefaultSqlExecutor>();
            services.AddTransient<ISqlSaver, DefaultSqlSaver>();
            services.AddTransient<Application>();
        }
    }
}
