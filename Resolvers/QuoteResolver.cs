using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;
using System;

namespace MemoirsTheme.Resolvers
{
    public class QuoteResolver : IInlineContentItemsResolver<Quote>
    {
        public string Resolve(Quote data)
        {
            string attribution = "";
            if (!string.IsNullOrWhiteSpace(data.Attribution))
            {
                attribution = $" ⸺ <cite>{data.Attribution}</cite>";
            }

            return $"<blockquote><p>&ldquo;{ TrimParagraph(data.Content)}&rdquo;{attribution}</p></blockquote>";
        }

        private static string TrimParagraph(string content)
        {
            var result = content.Trim();
            if (content.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Substring(3);
            }
            if (content.EndsWith("</p>", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Substring(0, result.Length-4);
            }

            return result;
        }
    
    }
}