namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public partial class Page : IPageMetadata
    {
        public string Url => $"{UrlSlug}.html";
    }
}