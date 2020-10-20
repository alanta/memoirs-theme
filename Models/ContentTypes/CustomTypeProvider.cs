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
            {typeof(CodeSnippet), "code_snippet"},
            {typeof(Container), "container"},
            {typeof(CustomerCase), "customer_case"},
            {typeof(GithubGist), "github_gist"},
            {typeof(Home), "home"},
            {typeof(Post), "post"}
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