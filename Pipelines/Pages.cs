using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Modules;
using MemoirsTheme.Pipelines;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.SearchIndex;
using System.Linq;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class Pages : Pipeline
    {
        public Pages(IDeliveryClient deliveryClient, SiteSettings site)
        {
            InputModules = new ModuleList
            {
                new Kontent<Page>(deliveryClient)
                    .OrderBy(Post.TitleCodename, SortOrder.Descending)
                    .WithQuery(new DepthParameter(2), new IncludeTotalCountParameter()),
                new SetMetadata(nameof(Page.Tags),
                    KontentConfig.Get<Page,string[]>(post => post.Tags?.Select(t => t.Codename).ToArray())),
                new SetMetadata(nameof(Page.Categories),
                    KontentConfig.Get<Page,string[]>(post => post.Categories?.Select(t => t.Codename).ToArray())),
                new SetDestination(KontentConfig.Get((Page page) => new NormalizedPath(page.Url))),
                new SetMetadata(SearchIndex.SearchItemKey, Config.FromDocument((doc, ctx) =>
                {
                    var page = doc.AsKontent<Page>();
                    return new LunrIndexDocItem(doc, page.Title, page.Body)
                    {
                        Description = page.MetadataMetaDescription,
                        //Tags = string.Join(", ", post.Tags.Select( t => t.Name ))
                    };
                })),
            };

            ProcessModules = new ModuleList
            {
                new MergeContent(new ReadFiles( KontentConfig.Get<Page,string>( ViewForPage))),
                new RenderRazor()
                    .WithViewData("Title", KontentConfig.Get<Page,string>( p => p.Title ))
                    .WithViewData("SiteMetadata", site)
                    .WithModel(KontentConfig.As<Page>()),
                new KontentImageProcessor(),
                new OptimizeHtml()
            };

            OutputModules = new ModuleList
            {
                new WriteFiles(),
            };
        }

        public string ViewForPage(Page page)
        {
            return page.System.Codename switch
            {
                "contact" => "Contact.cshtml",
                _ => "Page.cshtml"
            };
        }
    }
}

    