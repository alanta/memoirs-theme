using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;

namespace Kentico.Kontent.Statiq.Memoirs.Models 
{
    public interface IPageMetadata
    {
        public string MetadataTwitterCreator { get;  }
        public string UrlPattern { get; }
        public string MetadataMetaKeywords { get; }
        public IEnumerable<IAsset> MetadataTwitterImage { get; }
        public string MetadataOgTitle { get; }
        public IEnumerable<IAsset> MetadataOgImage { get; }
        public string MetadataTwitterSite { get; }
        public string MetadataMetaDescription { get; }
        public string MetadataMetaTitle { get; }
        public string MetadataOgDescription { get; }

        public string Title { get; }
        public IEnumerable<IAsset> TeaserImage { get;  }
        public IEnumerable<IMultipleChoiceOption> MetadataTwitterCard { get; set; }
    }
}