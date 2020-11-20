using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;
using System.Linq;
using System.Web;

namespace MemoirsTheme.Resolvers
{
    public class CodeSnippetResolver : IInlineContentItemsResolver<CodeSnippet>
    {
        public string Resolve(CodeSnippet data)
        {
            return $"<pre><code class=\"language-{data.Language.First().Codename}\">{HttpUtility.HtmlEncode(data.Code)}</code></pre>";
        }
    }
}