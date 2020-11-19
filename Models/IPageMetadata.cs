using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;

namespace Kentico.Kontent.Statiq.Memoirs.Models 
{
    public interface IPageMetadata
    {
        public string MetadataTwitterCreator { get;  }

        public string Url { get; }
        public string UrlSlug { get; }
        public string MetadataMetaKeywords { get; }
        public IEnumerable<IAsset> MetadataTwitterImage { get; }
        public string MetadataOgTitle { get; }
        public IEnumerable<IAsset> MetadataOgImage { get; }
        public string MetadataTwitterSite { get; }
        public string MetadataMetaDescription { get; }
        public string MetadataMetaTitle { get; }
        public string MetadataOgDescription { get; }

        public string Title { get; }
        public string Body { get; }
        public IEnumerable<IAsset> Image { get;  }
        public IEnumerable<IMultipleChoiceOption> MetadataTwitterCard { get; set; }
        public IEnumerable<IMultipleChoiceOption> Settings { get; set; }
    }
}