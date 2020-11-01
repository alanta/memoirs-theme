namespace Kentico.Kontent.Statiq.Memoirs.Models
{
    public class PostViewModel
    {
        public PostViewModel(Post post, Author author, SiteSettings metadata)
        {
            Post = post;
            Author = author;
            Metadata = metadata;
        }
        public Post Post { get; }
        public Author Author { get; }
        public SiteSettings Metadata { get; }
    }
}