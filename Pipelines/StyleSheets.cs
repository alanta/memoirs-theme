using Kentico.Kontent.Statiq.Memoirs.Models;
using Statiq.Common;
using Statiq.Core;
using Statiq.Sass;

namespace MemoirsTheme.Pipelines
{
    public class StyleSheets : Pipeline
    {
        public StyleSheets(SiteSettings site)
        {
            InputModules = new ModuleList
            {
                new ReadFiles("_sass/**/{!_,}*.scss"),
                CompileSass(site.OptimizeOutput),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                    new NormalizedPath($"assets/css/{doc.Source.FileNameWithoutExtension}.css"))),
                new WriteFiles()
            };
        }

        private CompileSass CompileSass(bool optimize)
        {
            var module = new CompileSass();
            return optimize ? module.WithCompressedOutputStyle() : module.WithCompactOutputStyle();
        }
    }
}