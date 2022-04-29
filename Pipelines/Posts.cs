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
    public class Posts : Pipeline
    {
        public Posts(IDeliveryClient deliveryClient, SiteSettings site)
        {
            Dependencies.Add(nameof(Seo));
            InputModules = new ModuleList{
                new Kontent<Post>(deliveryClient)
                    .WithQuery( 
                           new DepthParameter(2), 
                           new IncludeTotalCountParameter(), 
                           new OrderParameter("elements."+Post.PostDateCodename, SortOrder.Descending)),
                new SetMetadata(nameof(Post.Tags),
                    KontentConfig.Get<Post,ITaxonomyTerm[]>(post => post.Tags?.ToArray())),
                new SetMetadata(nameof(Post.Categories),
                    KontentConfig.Get<Post,ITaxonomyTerm[]>(post => post.Categories?.ToArray())),
                new SetDestination(KontentConfig.Get((Post post)  => new NormalizedPath(post.Url))),
                new SetMetadata(SearchIndex.SearchItemKey, Config.FromDocument((doc,ctx)=>
                {
                    var post = doc.AsKontent<Post>();
                    return new LunrIndexDocItem(doc, post.Title, post.Body)
                    {
                        Description = post.MetadataMetaDescription,
                        Tags = string.Join(", ", post.Tags.Select( t => t.Name ))
                    };
                })),
            };

            ProcessModules = new ModuleList {
                new MergeContent(new ReadFiles(patterns: "post.cshtml") ),
                new RenderRazor()
                    .WithViewData( "SEO", Config.FromDocument((doc, ctx) =>
                    {
                        var home = ctx.Outputs.FromPipeline(nameof(Seo)).First().AsKontent<Kentico.Kontent.Statiq.Memoirs.Models.Home>();
                        var post = doc.AsKontent<Post>();

                        return new SocialSharingMetadata(home, post);

                    }) )
                    .WithViewData("Title", KontentConfig.Get<Post,string>( p => p.Title ))
                    .WithViewData("Author", KontentConfig.Get<Post,Author>( p => p.Author.OfType<Author>().FirstOrDefault() ))
                    .WithViewData("SiteMetadata", site )
                    .WithModel(KontentConfig.As<Post>()),
                new KontentImageProcessor(),
                new OptimizeHtml(site.OptimizeOutput)
                    .WithSettings(settings =>
                    {
                        // conflicts with ratings
                        settings.RemoveScriptStyleTypeAttribute = false;
                        settings.MinifyJs = false; 
                    })
            };

            OutputModules = new ModuleList {
                new WriteFiles(),
            };
        }
    }
}
