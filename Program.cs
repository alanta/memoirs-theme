using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Common;
using Microsoft.Extensions.Configuration;
using Statiq.Web;
using Kentico.Kontent.Statiq.Memoirs.Models;

namespace MemoirsTheme
{
    public static class Program
    {
        public static async Task<int> Main(string[] args) =>
            await Bootstrapper
                .Factory
                .CreateDefault(args)
                .ConfigureServices((services, settings) =>
                {
                    services.AddSingleton<ITypeProvider, CustomTypeProvider>();
                    services.AddDeliveryClient((IConfiguration)settings);
                })
                .AddHostingCommands()
                .RunAsync();
    }
}
