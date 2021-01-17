using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using People.Client.Interfaces;
using People.Client.Services;

namespace People.Client
{
    class Program
    {
        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            IPrinter _printer = new PrintToConsole();

            try
            {
                await serviceProvider.GetService<IPeopleGetService>().RunTasks(_printer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPrinter, PrintToConsole>();

            serviceCollection.AddScoped<IPeopleGetService, PeopleGetService>();
        }
    }
}