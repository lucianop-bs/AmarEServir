using AmarEServir.Core.Common;

namespace AmarEServir.Core.Extesions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> source,
            int pageNumbers,
            int pageSize)
        {
            if (pageNumbers <= 0)
            {
                pageNumbers = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var totalCounts = source.Count();
            var items = source
                .Skip((pageNumbers - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PagedResult<T>(items, totalCounts, pageNumbers, pageSize));
        }
    }
}