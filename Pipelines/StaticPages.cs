using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using MemoirsTheme.Modules;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class StaticPages : Pipeline
    {
        public StaticPages(SiteSettings site)
        {
            InputModules = new ModuleList
            {
                new ReadFiles(patterns: "404.cshtml"),
                new SetDestination( Config.FromDocument( (doc,ctx) => new NormalizedPath(doc.Source.FileNameWithoutExtension+".html")))
            };

            ProcessModules = new ModuleList {
                new RenderRazor()
                    .WithViewData("SiteMetadata", site ),
                new KontentImageProcessor(),
                new OptimizeHtml()
            };

            OutputModules = new ModuleList {
                new WriteFiles(),
            };
        }
    }
}