using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Modules;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.SearchIndex;
using System.Linq;

namespace MemoirsTheme.Pipelines
{
    public class Posts : Pipeline
    {
        public Posts(IDeliveryClient deliveryClient, SiteSettings site)
        {
            InputModules = new ModuleList{
                new Kontent<Post>(deliveryClient)
                    .OrderBy(Post.PostDateCodename, SortOrder.Descending)
                    .WithQuery(new DepthParameter(2), new IncludeTotalCountParameter()),
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
                new MergeContent(new ReadFiles(patterns: "Post.cshtml") ),
                new RenderRazor()
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