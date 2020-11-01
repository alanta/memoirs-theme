namespace MemoirsTheme.Models
{
    public interface IPagedContent
    {
        int Index { get; }
        int TotalPages { get; }
        int TotalItems { get; }
        string Url { get; }
        string? PreviousUrl { get; }
        string? NextUrl { get; }
        string PagingUrl(in int index);
    }
}