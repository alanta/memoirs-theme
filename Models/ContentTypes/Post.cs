using System;
using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;

namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public partial class Post: IPageMetadata
    {
        public string Url => $"post/{PostDate.Value.Year}/{PostDate.Value.Month:00}/{UrlSlug}.html";

    }
}