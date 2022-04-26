using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Modules;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System.Collections.Generic;
using System.Linq;

namespace MemoirsTheme.Pipelines
{
    public class Taxonomy : Pipeline
    {
        public Taxonomy(SiteSettings site)
        {
            Dependencies.Add(nameof(Posts));

            ProcessModules = new ModuleList()
            {
                new ReplaceDocuments(
                    Config.FromContext(ctx =>
                    {
                        var docs =
                            // pull in documents from dependencies
                            ctx.Outputs.FromPipelines(Dependencies.ToArray());

                        return (IEnumerable<IDocument>) new[]
                        {
                            ctx.CreateDocument(
                                new NormalizedPath("categories.html"),
                                new[]
                                {
                                    // Stick it in a single document
                                    new KeyValuePair<string, object>(Keys.Children, docs),
                                    new KeyValuePair<string, object>(Keys.GroupKey, "Categories"),
                                    new KeyValuePair<string, object>(Keys.Title, "Categories")
                                }),
                            ctx.CreateDocument(
                                new NormalizedPath("tags.html"),
                                new[]
                                {
                                    // Stick it in a single document
                                    new KeyValuePair<string, object>(Keys.Children, docs),
                                    new KeyValuePair<string, object>(Keys.GroupKey, "Tags"),
                                    new KeyValuePair<string, object>(Keys.Title, "Tags")
                                }),
                        };

                    }))
            };

            OutputModules = new ModuleList()
            {
                new MergeContent(new ReadFiles(patterns: "grouped.cshtml")),
                new RenderRazor()
                    .WithViewData("Title", Config.FromDocument( (doc, ctx) => doc[Keys.Title]))
                    .WithViewData("SiteMetadata", site)
                    .WithModel(Config.FromDocument((doc, ctx) => doc.GetChildren()
                        // Group by category
                        .ToLookupMany(doc.Get<string>(Keys.GroupKey), new TaxonomyTermComparer() ))),
                new KontentImageProcessor(),
                new OptimizeHtml(site.OptimizeOutput)
                    .WithSettings(settings =>
                    {
                        // conflicts with ratings
                        settings.RemoveScriptStyleTypeAttribute = false;
                        settings.MinifyJs = false;
                    }),
                new WriteFiles()
            };
        }
    }
}
