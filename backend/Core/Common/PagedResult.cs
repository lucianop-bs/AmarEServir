namespace AmarEServir.Core.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; }
        public int PageNumbers { get; }
        public int PageSize { get; }
        public int TotalCounts { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageNumbers > 1;
        public bool HasNextPage => PageNumbers < TotalPages;

        public PagedResult(List<T> items, int totalCounts, int pageNumbers, int pageSize)
        {
            Items = items;
            TotalCounts = totalCounts;
            PageNumbers = pageNumbers;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCounts / (double)pageSize);
        }
    }
}