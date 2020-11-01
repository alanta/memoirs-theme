using Kentico.Kontent.Statiq.Memoirs.Models;
using Kontent.Statiq;
using Statiq.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoirsTheme.Models
{

    public class PagedContent<TContentModel> : IPagedContent
        where TContentModel : IPageMetadata
    {
        private readonly IDocument _document;
        private readonly Func<int, string> _pagingUrl;
        private readonly Lazy<IReadOnlyList<TContentModel>> _children;
        private readonly Lazy<PagedContent<TContentModel>> _previous;
        private readonly Lazy<PagedContent<TContentModel>> _next;

        public PagedContent(IDocument document, Func<int,string> pagingUrl)
        {
            _document = document;
            _pagingUrl = pagingUrl;
            _children = new Lazy<IReadOnlyList<TContentModel>>(() => document.GetChildren().Select(c => TypedContentExtensions.AsKontent<TContentModel>(c)).ToArray());

            _previous = new Lazy<PagedContent<TContentModel>>(() =>
            {
                var otherDocument = document.GetDocument(Keys.Previous);
                return otherDocument != null ? new PagedContent<TContentModel>(otherDocument, pagingUrl) : null;
            });
            _next = new Lazy<PagedContent<TContentModel>>(() => {
                var otherDocument = document.GetDocument(Keys.Next);
                return otherDocument != null ? new PagedContent<TContentModel>(otherDocument, pagingUrl) : null;
            });
        }

        public int Index => _document.GetInt(Keys.Index);
        public int TotalPages => _document.GetInt(Keys.TotalPages);
        public int TotalItems => _document.GetInt(Keys.TotalItems);
        public IReadOnlyList<TContentModel> Items => _children.Value;
        public PagedContent<TContentModel> Previous => _previous.Value;
        public PagedContent<TContentModel> Next => _next.Value;
        public string Url => _document.GetLink();
        public string? PreviousUrl => _previous?.Value?.Url;
        public string? NextUrl => _next?.Value?.Url;
        public string PagingUrl(in int index) => _pagingUrl(index);


    }
}