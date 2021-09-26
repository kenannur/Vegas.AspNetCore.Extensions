using System.Linq;

namespace Vegas.NetCore.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> NextPage<TEntity>(this IQueryable<TEntity> queryable, int? pageNumber, int? pageCount)
        {
            if (!pageNumber.HasValue || !pageCount.HasValue)
            {
                return queryable;
            }
            if (pageNumber.Value <= 0 || pageCount.Value <= 0)
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }
            return queryable
                .Skip((pageNumber.Value - 1) * pageCount.Value)
                .Take(pageCount.Value);
        }
    }
}
