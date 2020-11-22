using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;
using System.Linq;

namespace MemoirsTheme.Models
{
    public class SocialSharingMetadata
    {
        public SocialSharingMetadata(IPageMetadata site, IPageMetadata page)
        {
            Title = page.MetadataOgTitle.Cascade(page.MetadataMetaTitle).Cascade(page.Title);
            Url = page.Url;
            TwitterCard = page.MetadataTwitterCard.FirstOrDefault()?.Codename ?? "summary_large_image";
            Description = page.MetadataOgDescription
                .Cascade(page.MetadataMetaDescription);
            Image = page.MetadataOgImage.FirstOrDefault() ?? site?.MetadataOgImage.FirstOrDefault();
            TwitterSite = page.MetadataTwitterSite
                .Cascade(site?.MetadataTwitterSite);
            TwitterCreator = page.MetadataTwitterCreator
                .Cascade(site?.MetadataTwitterCreator);
            TwitterImage = page.MetadataTwitterImage.FirstOrDefault() ?? Image;
        }

        public string Url { get; }
        public string Title { get; }
        public string Description { get; }
        public IAsset Image { get; }
        public IAsset TwitterImage { get; }
        public string TwitterCreator { get; }
        public string TwitterSite { get; }
        public string TwitterCard { get; set; }
    }

    public static class MetadataHelpers
    {
        public static string Cascade(this string preferred, string fallback)
        {
            return string.IsNullOrWhiteSpace(preferred) ? fallback : preferred;
        }
    }
}