using MemoirsTheme.Pipelines;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Index = Lunr.Index;

namespace MemoirsTheme.Modules
{
    public class LunrIndexer : Module
    {
        private static readonly Regex StripHtmlAndSpecialChars = new Regex(@"<[^>]+>|&[a-zA-Z]{2,};|&#\d+;|[^a-zA-Z-#]", RegexOptions.Compiled);

        protected override async Task<IEnumerable<IDocument>> ExecuteContextAsync(IExecutionContext context)
        {
            if (context.Inputs.Length == 0)
            {
                throw new InvalidOperationException($"No input documents available. Cannot build a search index for pipeline {context.PipelineName}");
            }

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
                    var searchItem = doc[SearchIndex.SearchItemKey] as LunrIndexDocItem;
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

            return new[] { context.CreateDocument(destination: "index.js", 
                $@"var documents={System.Text.Json.JsonSerializer.Serialize(documents)};{Environment.NewLine}var data='{idx.ToJson()}';{Environment.NewLine}") };

        }
    }
}