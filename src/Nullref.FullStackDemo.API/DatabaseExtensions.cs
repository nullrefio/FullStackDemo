namespace Nullref.FullStackDemo.API
{
    public static class DatabaseExtensions
    {
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static IQueryable<TModel> ApplyPaging<TModel>(this IQueryable<TModel> source, IPagingCriteriaModel model)
            where TModel : IDatabaseItem => source.Skip(model.StartIndex()).Take(model.PageSize);

        public static PaginatedResponseModel<TModel>
            ToPagedModel<TEntity, TModel>(this IQueryable<TEntity> query, IPagingCriteriaModel model, Func<TEntity, TModel> mapper)
               where TModel : class, IModel
               where TEntity : class, IDatabaseItem, new()
        {
            var items = query.ApplyPaging(model).ToList();
            return new PaginatedResponseModel<TModel>
            {
                Items = items.AsParallel().AsOrdered().Select(x => mapper(x)).ToList(),
                PageSize = model.PageSize,
                PageNumber = model.PageNumber,
                TotalItems = query.Count(),
                Search = model.Search,
                Order = model.Order,
            };
        }
    }
}
