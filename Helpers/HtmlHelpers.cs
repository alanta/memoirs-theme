using AngleSharp.Dom;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Urls.ImageTransformation;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Statiq.Common;
using System.Collections.Generic;
using System.Linq;
using IHtmlContent = Microsoft.AspNetCore.Html.IHtmlContent;

namespace MemoirsTheme.Helpers
{
    public static class HtmlHelpers
    {
        public static string GetLink(this IHtmlHelper html, string relativeUri, bool includeHost = false)
        {
            var context = html.ViewData["StatiqExecutionContext"] as IExecutionContext;
            return context.GetLink(relativeUri, includeHost);
        }

        public static SiteSettings Site(this IHtmlHelper html)
        {
            return html.ViewData["SiteMetaData"] as SiteSettings;
        }

        public static IHtmlContent Image(this IHtmlHelper html, IEnumerable<IAsset> assets, string? description = null,
            bool lazy = false,
            int? width = null, int? height = null, ImageFitMode? fit = null, object? htmlAttributes = null)
        {
            if (assets == null || !assets.Any())
            {
                return HtmlString.Empty;
            }

            return html.Image(assets.First(), description, lazy, width, height, fit, htmlAttributes);
        }

        public static IHtmlContent Image(this IHtmlHelper html, IAsset asset, string? description = null,
            bool lazy = false, int? width = null, int? height = null, ImageFitMode? fit = null,
            object? htmlAttributes = null)
        {
            var tag = new TagBuilder("img");

            var imageUrl = new ImageUrlBuilder(asset.Url);
            if (width.HasValue)
            {
                imageUrl = imageUrl.WithWidth(width.Value);
            }

            if (height.HasValue)
            {
                imageUrl = imageUrl.WithHeight(height.Value);
            }

            if (fit != null)
            {
                imageUrl = imageUrl.WithFitMode(fit.Value);
            }

            tag.MergeAttributes(Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            if (lazy)
            {
                tag.AddCssClass("lazyimg");
                tag.Attributes["src"] =
                    "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAMAAAACCAQAAAA3fa6RAAAADklEQVR42mNkAANGCAUAACMAA2w/AMgAAAAASUVORK5CYII=";
                tag.Attributes["data-src"] = imageUrl.Url.ToString();
            }
            else
            {
                tag.Attributes["src"] = imageUrl.Url.ToString();
            }

            tag.Attributes["alt"] = description ?? asset.Description;
            return tag.RenderSelfClosingTag();
        }


        public static IHtmlContent TableOfContents(this IHtmlHelper htmlHelper, string content)
        {
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            var html = parser.ParseDocument(content);

            var toc = new Stack<(TagBuilder tag, int level)>();
            toc.Push((new TagBuilder("ul"), 0));

            var headings = EnumerateHeadings(html.Children);
            foreach (var heading in headings)
            {
                switch (heading.TagName)
                {
                    case "H1":
                        // Shouldn't happen
                        break;
                    case "H2":
                        toc.Unwind(0);
                        // add child
                        var h2 = new TagBuilder("li");
                        h2.InnerHtml.AppendHtml(heading.InnerHtml);
                        toc.Push((h2, 1));

                        break;
                    case "H3":
                        toc.Unwind(2);

                        if (toc.Peek().tag.TagName != "ul")
                            toc.Push((new TagBuilder("ul"), 2));

                        // add child
                        var h3 = new TagBuilder("li");
                        h3.InnerHtml.AppendHtml(heading.InnerHtml);
                        toc.Push((h3, 3));

                        break;
                    case "H4":
                        toc.Unwind(4);

                        if (toc.Peek().tag.TagName != "ul")
                            toc.Push((new TagBuilder("ul"), 3));

                        // add child
                        var h4 = new TagBuilder("li");
                        h4.InnerHtml.AppendHtml(heading.InnerHtml);
                        toc.Push((h4, 4));

                        break;
                }
            }

            // unwind the stack
            toc.Unwind(0);

            return toc.Pop().tag;
        }

        private static IEnumerable<IElement> EnumerateHeadings(IEnumerable<IElement> elements)
        {
            if (elements == null)
                yield break;

            foreach (var element in elements)
            {
                switch (element.TagName)
                {
                    case "H1":
                    case "H2":
                    case "H3":
                    case "H4":
                    case "H5":
                    case "H6":
                        yield return element;
                        break;
                }

                foreach (var child in EnumerateHeadings(element.Children))
                {
                    yield return child;
                }
            }
        }

        private static void Unwind(this Stack<(TagBuilder tag, int level)> stack, int level)
        {
            while (stack.Peek().level > level)
            {
                var inner = stack.Pop().tag;
                stack.Peek().tag.InnerHtml.AppendHtml(inner);
            }
        }
    }
}
    
