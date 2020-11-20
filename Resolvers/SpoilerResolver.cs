using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;

namespace MemoirsTheme.Resolvers
{
    public class SpoilerResolver : IInlineContentItemsResolver<Spoiler>
    {
        public string Resolve(Spoiler data)
        {
            return $"<div class=\"spoiler\">{data.Content}</div>";
        }
    }
}