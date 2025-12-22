namespace CBVSignalR.Application.Models
{
    public class PagingFilterRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Keyword { get; set; }
        public string? SortBy { get; set; }
        public string? SortDir { get; set; }
    }
}
