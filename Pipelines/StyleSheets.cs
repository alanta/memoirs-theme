using Statiq.Common;
using Statiq.Core;
using Statiq.Sass;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class StyleSheets : Pipeline
    {
        public StyleSheets()
        {
            InputModules = new ModuleList
            {
                new ReadFiles("_sass/**/{!_,}*.scss"),
                new CompileSass()
                    .WithCompactOutputStyle(),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                    new NormalizedPath($"assets/css/{doc.Source.FileNameWithoutExtension}.css"))),
                new WriteFiles()
            };
        }
    }
}