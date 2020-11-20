namespace Kentico.Kontent.Statiq.Memoirs.Models{
    public class SiteSettings{
        public string Name {get; set;}
        public string Title {get; set;}
        public string Description {get; set;}
        public string Logo {get; set;}
        public string Favicon {get; set;}
        public bool LazyImages { get; set; }
        public bool ImageShadow { get; set; }
        public string Copyright { get; set; }
        public bool AuthorBox { get; set; }
        public string DisqusId { get; set; }
        public string ContactFormUrl { get; set; }
        public string? GoogleAnalytics { get; set; }
        public bool OptimizeOutput { get; set; } = true;
    }
}