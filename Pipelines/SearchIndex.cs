using MemoirsTheme.Modules;
using Statiq.Common;
using Statiq.Core;
using System.Linq;
using Pipeline = Statiq.Core.Pipeline;

namespace MemoirsTheme.Pipelines
{
    public class SearchIndex : Pipeline
    {
        public const string SearchItemKey = "SearchItem";
        public SearchIndex()
        {
            Dependencies.AddRange(nameof(Posts));
            PostProcessModules = new ModuleList(
                // pull documents from other pipelines
                new ReplaceDocuments(Dependencies.ToArray()),
                new LunrIndexer(),
                new AppendContent(Config.FromContext(ctx => ctx.FileSystem.GetInputFile("assets/js/lunrsearchengine.js").ReadAllTextAsync())),
                new SetDestination("assets/js/lunrsearchengine.js")
                    
            );
            OutputModules = new ModuleList(

                new WriteFiles()
            );
        }
    }
}


