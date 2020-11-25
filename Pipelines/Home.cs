using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Models;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System.Linq;

namespace MemoirsTheme.Pipelines
{
    public class Home : Pipeline
    {
        public Home(SiteSettings site)
        {
            Dependencies.Add(nameof(Posts));
            ProcessModules = new ModuleList(
                // pull documents from other pipelines
                new ReplaceDocuments(Dependencies.ToArray()),
                new PaginateDocuments(6),
                new SetDestination(Config.FromDocument((doc, ctx) => Filename(doc))),
                new MergeContent(new ReadFiles(patterns: "index.cshtml")),
                new RenderRazor()
                    .WithViewData("Title", "Home")
                    .WithViewData("SiteMetaData", site)
                    .WithModel(Config.FromDocument((document, context) => new PagedContent<Post>(document, PagingUrl))),
                new KontentImageProcessor()

            );

            OutputModules = new ModuleList {
                new WriteFiles(),
            };
        }

        private static string PagingUrl(int index) => index > 1 ? $"page{index}.html" : "index.html";

        private static NormalizedPath Filename(IDocument document)
        {
            var index = document.GetInt(Keys.Index);

            return new NormalizedPath(PagingUrl(index));
        }
    }
}
