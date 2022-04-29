using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Urls.Delivery.QueryParameters;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Models;
using MemoirsTheme.Modules;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System.Linq;

namespace MemoirsTheme.Pipelines
{
    public class Pages : Pipeline
    {
        public Pages(IDeliveryClient deliveryClient, SiteSettings site)
        {
            Dependencies.Add(nameof(Seo));
            InputModules = new ModuleList
            {
                new Kontent<Page>(deliveryClient)
                    .WithQuery(
                        new DepthParameter(2), 
                        new IncludeTotalCountParameter(), 
                        new OrderParameter("elements."+Post.TitleCodename, SortOrder.Descending)),
                new SetMetadata(nameof(Page.Tags),
                    KontentConfig.Get<Page,ITaxonomyTerm[]>(post => post.Tags?.ToArray())),
                new SetMetadata(nameof(Page.Categories),
                    KontentConfig.Get<Page,ITaxonomyTerm[]>(post => post.Categories?.ToArray())),
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
                new MergeContent(new ReadFiles( "page.cshtml")),
                new RenderRazor()
                    .WithViewData( "SEO", Config.FromDocument((doc, ctx) =>
                    {
                        var home = ctx.Outputs.FromPipeline(nameof(Seo)).First().AsKontent<Kentico.Kontent.Statiq.Memoirs.Models.Home>();
                        var post = doc.AsKontent<Page>();

                        return new SocialSharingMetadata(home, post);

                    }) )
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
    }
}

    