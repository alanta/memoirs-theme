using MemoirsTheme.Models;
using Statiq.Common;
using System;

namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public class HomeViewModel : PagedContent<Post>
    {
        public HomeViewModel(IDocument document, Func<int, string> pagingUrl,  SiteSettings metadata) : base(document, pagingUrl)
        {
            Metadata = metadata;
        }

        public SiteSettings Metadata { get; }
    }
}