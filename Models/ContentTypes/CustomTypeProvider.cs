using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Kontent.Delivery.Abstractions;

namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public class CustomTypeProvider : ITypeProvider
    {
        private static readonly Dictionary<Type, string> _codenames = new Dictionary<Type, string>
        {
            {typeof(Author), "author"},
            {typeof(CodeSnippet), "code_snippet"},
            {typeof(Contact), "contact"},
            {typeof(GithubGist), "github_gist"},
            {typeof(Home), "home"},
            {typeof(Page), "page"},
            {typeof(Post), "post"},
            {typeof(Quote), "quote"},
            {typeof(Spoiler), "spoiler"}
        };

        public Type GetType(string contentType)
        {
            return _codenames.Keys.FirstOrDefault(type => GetCodename(type).Equals(contentType));
        }

        public string GetCodename(Type contentType)
        {
            return _codenames.TryGetValue(contentType, out var codename) ? codename : null;
        }
    }
}