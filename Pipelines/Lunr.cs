using Jint.Native.Json;
using Kentico.Kontent.Statiq.Lumen.Pipelines;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Statiq.Common;
using Statiq.Core;
using Statiq.SearchIndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pipeline = Statiq.Core.Pipeline;
using Index = Lunr.Index;

namespace MemoirsTheme.Pipelines
{
    public class SearchIndex : Pipeline
    {
        public const string SearchItemKey = GenerateLunrIndexKeys.LunrIndexItem;
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

    public class LunrIndexer : Module
    {
        private static readonly Regex StripHtmlAndSpecialChars = new Regex(@"<[^>]+>|&[a-zA-Z]{2,};|&#\d+;|[^a-zA-Z-#]", RegexOptions.Compiled);

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            var documents = new Dictionary<string,object>();

            Index idx = await Index.Build(async builder =>
            {
                builder.ReferenceField = "id";

                builder
                    .AddField("title")
                    .AddField("content")
                    .AddField("tags")
                    .AddField("description");

                foreach (IDocument doc in context.Inputs)
                {
                    var searchItem = doc[SearchIndex.SearchItemKey] as ILunrIndexItem;
                    if (searchItem == null)
                        continue;

                    var id = Guid.NewGuid().ToString("D");
                    
                    var content = StripHtmlAndSpecialChars.Replace(searchItem.Content, " ").Trim();

                    documents.Add(id, new 
                    {
                        url = searchItem.GetLink(context, false),
                        title = searchItem.Title,
                        body = searchItem.Description
                    });
                    
                    var lunrDoc = new global::Lunr.Document
                    {
                        {"id", id},
                        {"title", searchItem.Title},
                        {"content", content},
                        {"tags", searchItem.Tags},
                        {"description", searchItem.Description}
                    };
                    await builder.Add(lunrDoc);
                    
                }
            });

            return new[] { (await context.CreateDocumentAsync(destination: "index.js", 
                $@"var documents={System.Text.Json.JsonSerializer.Serialize(documents)};{Environment.NewLine}var data='{idx.ToJson()}';{Environment.NewLine}")) };

        }
    }
}


