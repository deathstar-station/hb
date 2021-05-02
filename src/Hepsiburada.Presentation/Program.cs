using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Hepsiburada.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetService<App>();

            await app.Run();
        }
    }
}
