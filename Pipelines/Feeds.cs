using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using Microsoft.Extensions.Logging;
using Statiq.Common;
using Statiq.Core;
using Statiq.Feeds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoirsTheme.Pipelines
{
    public class Feeds : Pipeline
    {
        public Feeds(SiteSettings site)
        {
            Dependencies.Add(nameof(Posts));
            ProcessModules = new ModuleList(
                // pull documents from other pipelines
                new ReplaceDocuments(Dependencies.ToArray()),
                
                // Set metadata for the feeds module
                new SetMetaDataItems(
                    async (input, context) =>
                    {
                        var post = input.AsKontent<Post>();
                        var html = await ParseHtml(input, context);
                        var article = html?.GetElementsByTagName("article").FirstOrDefault()?.InnerHtml ?? "";
                        
                        var metadata = new MetadataItems
                        {
                            {FeedKeys.Title, post.Title},
                            {FeedKeys.Description, post.MetadataMetaDescription},
                            {FeedKeys.Published, post.PostDate},
                            {FeedKeys.Content, article}
                        };

                        AddImage(metadata, post.MetadataOgImage.FirstOrDefault() ?? post.Image.FirstOrDefault());

                        return metadata;
                    })
            );
            OutputModules = new ModuleList(
                new GenerateFeeds()
                    .WithFeedTitle(site.Title)
                    .WithFeedDescription(site.Description)
                    .WithFeedCopyright(site.Copyright),
                new WriteFiles()
            );
        }

        private static void AddImage(MetadataItems metadata, IAsset? asset)
        {
            if (asset == null) return;

            var localPath = KontentAssetHelper.GetLocalFileName(asset.Url+"?w=800&h=800", "img");
            var download = new KontentImageDownload(asset.Url, localPath);

            metadata.Add(FeedKeys.Image, localPath);
            metadata.Add("KONTENT-ASSET-DOWNLOADS", download); // TODO : an upcoming version of Kontent.Statiq will provide this key
        }

        private static async Task<IHtmlDocument?> ParseHtml(IDocument document, IExecutionContext context)
        {
            var parser = new HtmlParser();
            try
            {
                await using var stream = document.GetContentStream();
                return await parser.ParseDocumentAsync(stream);
            }
            catch (Exception ex)
            {
                context.LogWarning("Exception while parsing HTML for {0}: {1}", document.ToSafeDisplayString(), ex.Message);
            }

            return null;
        }
    }

    public class SetMetaDataItems : Module
    {
        private readonly Func<IDocument, IExecutionContext, Task<MetadataItems>> _getMetadata;

        public SetMetaDataItems(Func<IDocument, IExecutionContext, Task<MetadataItems>> getMetadata)
        {
            _getMetadata = getMetadata;
        }

        protected override async Task<IEnumerable<IDocument>> ExecuteInputAsync(IDocument input, IExecutionContext context)
        {
            var metadata = await _getMetadata(input, context);

            return metadata == null
                ? input.Yield()
                : input.Clone(metadata).Yield();
        }
    }
}