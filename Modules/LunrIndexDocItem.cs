using Statiq.Common;

namespace MemoirsTheme.Modules
{
    /// <summary>
    /// A search item for a document.
    /// </summary>
    public class LunrIndexDocItem
    {
        /// <summary>
        /// The document the search item points to.
        /// </summary>
        public IDocument Document { get; set; }

        /// <summary>
        /// The title of the search item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The description of the search item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The content of the search item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Any tags for the search item.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Creates the search item.
        /// </summary>
        /// <param name="document">The document this search item should point to.</param>
        /// <param name="title">The title of the search item.</param>
        /// <param name="content">The search item content.</param>
        public LunrIndexDocItem(IDocument document, string title, string content)
        {
            Document = document;
            Title = title;
            Content = content;
            Tags = "";
            Description = "";
        }

        /// <summary>
        /// Gets a link to the search item result.
        /// </summary>
        /// <param name="context">The current execution context.</param>
        /// <param name="includeHost"><c>true</c> to include the hostname, <c>false otherwise</c>.</param>
        /// <returns>A link to the search item.</returns>
        public string GetLink(IExecutionContext context, bool includeHost) =>
            context.GetLink(Document, includeHost);
    }
}