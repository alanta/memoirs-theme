using Kentico.Kontent.Delivery.Abstractions;
using System.Threading.Tasks;

namespace MemoirsTheme.Resolvers
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        public Task<string> ResolveLinkUrlAsync(IContentLink link)
        {
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