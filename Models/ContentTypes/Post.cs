namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public partial class Post: IPageMetadata
    {
        public string Url => $"post/{PostDate?.Year ?? 0}/{(PostDate?.Month??0):00}/{UrlSlug}.html";

    }
}