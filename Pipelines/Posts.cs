using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Pipelines;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using Statiq.SearchIndex;
using System.Linq;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
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
                    KontentConfig.Get<Post,string[]>(post => post.Tags?.Select(t => t.Codename).ToArray())),
                new SetMetadata(nameof(Post.Categories),
                    KontentConfig.Get<Post,string[]>(post => post.Categories?.Select(t => t.Codename).ToArray())),
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
                new KontentImageProcessor()
            };

            OutputModules = new ModuleList {
                new WriteFiles(),
            };
        }
    }
}