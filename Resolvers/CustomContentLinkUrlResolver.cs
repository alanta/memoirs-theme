using Kentico.Kontent.Delivery.Abstractions;
using System.Threading.Tasks;

namespace MemoirsTheme.Resolvers
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        public Task<string> ResolveLinkUrlAsync(IContentLink link)
        {
            // Resolves URLs to content items based on the 'accessory' content type
            var result = link.ContentTypeCodename switch
            {
                "post" => $"{link.UrlSlug}",
                _ => "/404",
            };

            return Task.FromResult(result);
        }

        public Task<string> ResolveBrokenLinkUrlAsync()
        {
            // Resolves URLs to unavailable content items
            return Task.FromResult("/404");
        }
    }
}