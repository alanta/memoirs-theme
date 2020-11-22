using System;
using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;

namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public partial class Home : IPageMetadata
    {
        public string Url { get; } = "/";
        public string Title { get; } = "Home";
        public string Body => this.Contact;
        public IEnumerable<IAsset> Image { get; } = Array.Empty<IAsset>();
        public IEnumerable<IMultipleChoiceOption> Settings { get; set; } = Array.Empty<IMultipleChoiceOption>();
    }
}