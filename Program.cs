using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Common;
using Microsoft.Extensions.Configuration;
using Statiq.Web;
using Kentico.Kontent.Statiq.Memoirs.Models;
using MemoirsTheme.Resolvers;

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
                    // pull in site settings from configuration
                    var siteSettings = (settings as IConfiguration).GetSection("Site").Get<SiteSettings>();
                    services.AddSingleton(siteSettings);

                    services.AddSingleton<ITypeProvider, CustomTypeProvider>();
                    services.AddDeliveryInlineContentItemsResolver(new CodeSnippetResolver());
                    services.AddDeliveryInlineContentItemsResolver(new GitHubGistResolver());
                    services.AddDeliveryInlineContentItemsResolver(new SpoilerResolver());
                    services.AddDeliveryInlineContentItemsResolver(new QuoteResolver());
                    services.AddDeliveryClient((IConfiguration)settings);
                })
                .AddHostingCommands()
                .RunAsync();
    }
}
