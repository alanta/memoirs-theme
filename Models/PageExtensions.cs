using System.Linq;

namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public static class PageExtensions
    {
        public static bool TableOfContents(this IPageMetadata page)
        {
            return page.Settings?.Any(opt => opt.Codename == "toc") ?? false;
        }

        public static bool ImageShadow(this IPageMetadata page)
        {
            return page.Settings?.Any(opt => opt.Codename == "imageshadow") ?? false;
        }

        public static bool Comments(this IPageMetadata page)
        {
            return page.Settings?.Any(opt => opt.Codename == "comments") ?? false;
        }
    }
}