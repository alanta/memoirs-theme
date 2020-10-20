using Statiq.Common;
using Statiq.Core;
using Statiq.Sass;

namespace Kentico.Kontent.Statiq.Lumen.Pipelines
{
    public class StyleSheets : Pipeline
    {
        public StyleSheets()
        {
            InputModules = new ModuleList {
                new ReadFiles(pattern: "_sass/**/{!_,}*.scss"),
                new CompileSass()
                    .WithCompactOutputStyle(),
                new SetDestination(".css"),
                new WriteFiles()
            };
        }
    }
}