using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.ImageTransformation;
using Kentico.Kontent.Statiq.Memoirs.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

        public static IHtmlContent Image(this IHtmlHelper html, IEnumerable<IAsset> assets, string? description = null, bool lazy = false,
            int? width = null, int? height = null, ImageFitMode? fit = null, object? htmlAttributes = null)
        {
            if (assets == null || !assets.Any())
            {
                return HtmlString.Empty;
            }
            
            return html.Image(assets.First(), description, lazy, width, height, fit, htmlAttributes);
        }
        public static IHtmlContent Image(this IHtmlHelper html, IAsset asset, string? description = null, bool lazy = false, int? width = null, int? height = null, ImageFitMode? fit = null, object? htmlAttributes = null)
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

            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            if (lazy)
            {
                tag.AddCssClass("lazyimg");
                tag.Attributes["src"]="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAMAAAACCAQAAAA3fa6RAAAADklEQVR42mNkAANGCAUAACMAA2w/AMgAAAAASUVORK5CYII=";
                tag.Attributes["data-src"] = imageUrl.Url.ToString();
            }
            else
            {
                tag.Attributes["src"] = imageUrl.Url.ToString();
            }

            tag.Attributes["alt"] = description ?? asset.Description;
            return tag.RenderSelfClosingTag();
        }
    }
}
