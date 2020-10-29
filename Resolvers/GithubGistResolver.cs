using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Statiq.Memoirs.Models;

namespace MemoirsTheme.Resolvers
{
    public class GitHubGistResolver : IInlineContentItemsResolver<GithubGist>
    {
        public string Resolve(GithubGist data)
        {
            return $"<script src=\"https://gist.github.com/{data.Account}/{data.GistId}.js\"></script>";
        }
    }
}