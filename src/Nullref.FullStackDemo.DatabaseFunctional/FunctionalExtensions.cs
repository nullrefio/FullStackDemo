using Nullref.FullStackDemo.CommonModels;
using Nullref.FullStackDemo.CommonModels.Exceptions;
using Nullref.FullStackDemo.Database;
using Nullref.FullStackDemo.TextToQuery;
using System.Linq.Expressions;

namespace Nullref.FullStackDemo.DatabaseFunctional
{
    public static class FunctionalExtensions
    {
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static void ThrowInUse(this bool action)
        {
            if (action)
                ModelErrorCollection.ThrowInUse();
        }

        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static async Task ThrowInUseAsync(this Task<bool> action)
        {
            if (await action)
                ModelErrorCollection.ThrowInUse();
        }

        #region ThrowIfExists

        public static async Task ThrowIfExistsAsync<T>(this Task<T> action, string objectName)
            where T : class
        {
            if (await action != null) throw new EntityExistsException(objectName);
        }

        public static async Task ThrowIfExistsAsync(this Task<bool> action, string objectName)
        {
            if (await action) throw new EntityExistsException(objectName);
        }

        /// <summary>
        /// Raise error immediately if the item exists
        /// </summary>
        public static async Task ThrowIfExistsAsync<T, R>(this Task<R> action, System.Linq.Expressions.Expression<Func<R, T>> selector)
            where R : class
        {
            if (await action != null)
                throw new EntityExistsException(selector.Body.ToString().Split('.').LastOrDefault());
        }

        public static void ThrowIfExists(this bool action, string? objectName = null)
        {
            if (action) throw new EntityExistsException(objectName);
        }

        public static void ThrowIfExists<T>(this T action, string? objectName = null)
        {
            if (action != null) throw new EntityExistsException(objectName);
        }

        #endregion

        /// <summary>
        /// Use this for "AnyAsync" methods to throw an error on true statements
        /// </summary>
        public static async Task ThrowIfTrueAsync(this Task<bool> action, string objectName, string message)
        {
            if (await action)
                ModelErrorCollection.ThrowError(objectName, message);
        }

        public static void ThrowIfTrue(this bool action, string objectName, string message)
        {
            if (action)
                ModelErrorCollection.ThrowError(objectName, message);
        }

        /// <summary>
        /// Use this for "AnyAsync" methods to throw an error on false statements
        /// </summary>
        public static async Task ThrowIfFalseAsync(this Task<bool> action, string objectName, string message)
        {
            if (!await action)
                ModelErrorCollection.ThrowError(objectName, message);
        }

        public static async Task<int> ThrowIfDeleteFailedAsync(this Task<int> action, string? objectName = null)
        {
            var v = await action;
            if (v == 0) throw new EntityNotFoundException(objectName);
            return v;
        }

        #region ToPagedModel

        public static PaginatedResponseModel<TModel>
           ToPagedModel<TModel, TEntity>(this IDatabaseModelMapper<TModel, TEntity> query, IDataContext? context = null)
               where TModel : class, IModel
               where TEntity : class, IDatabaseItem, new()
        {
            var items = query.AsQuerable().ApplyPaging(query.Model).ToList();
            var count = query.AsQuerable().Count();

            return new PaginatedResponseModel<TModel>
            {
                Items = items.AsParallel().AsOrdered().Select(x => query.Mapper(x)).ToList(),
                PageSize = query.Model.PageSize,
                PageNumber = query.Model.PageNumber,
                TotalItems = count,
                Search = query.Model.Search,
                Order = query.Model.Order,
            };
        }

        #endregion

        #region Sorting Methods

        public static IQueryable<TEntity> ApplySort<TModel, TEntity>(
            this IQueryable<TEntity> source, ISearchCriteriaModel model)
            where TModel : IModel
            where TEntity : class, IDatabaseItem, new()
        {
            var sorter = new SortOptions<TModel, TEntity>(model.Order);
            var result = sorter.Apply(source);
            if (sorter.HasAppliedSort)
                model.Order = string.Join("|", sorter.AppliedSorts);
            return result;
        }

        public static IDatabaseModelMapper<TModel, TEntity> ApplySort<TModel, TEntity>(
            this IDatabaseModelMapper<TModel, TEntity> source)
            where TModel : IModel
            where TEntity : class, IDatabaseItem, new()
        {
            var sorter = new SortOptions<TModel, TEntity>(source.Model.Order, source.HasAppliedSort);
            var result = sorter.Apply(source.AsQuerable());
            source.ApplyQuery(result);
            if (sorter.HasAppliedSort)
            {
                source.HasAppliedSort = true;
                source.Model.Order = string.Join("|", sorter.AppliedSorts);
            }
            return source;
        }

        public static IDatabaseModelMapper<TModel, TEntity> ApplySort<TModel, TEntity, TKey>(this IDatabaseModelMapper<TModel, TEntity> source, Expression<Func<TModel, TKey>> keySelector, bool ascending = true)
            where TModel : IModel
            where TEntity : class, IDatabaseItem, new()
        {
            var field = keySelector.Body.ToString().Split('.').LastOrDefault();
            var sorter = new SortOptions<TModel, TEntity>(field + "," + (ascending ? "asc" : "desc"), source.HasAppliedSort);
            var result = sorter.Apply(source.AsQuerable());
            source.ApplyQuery(result);
            if (sorter.HasAppliedSort)
            {
                source.HasAppliedSort = true;
                source.Model.Order = string.Join("|", sorter.AppliedSorts);
            }
            return source;
        }

        #endregion

        #region Search Methods

        public static IQueryable<TEntity> ApplySearch<TModel, TEntity>(
            this IQueryable<TEntity> source, ISearchCriteriaModel model)
            where TModel : IModel
            where TEntity : class, IDatabaseItem, new()
        {
            var search = new SearchOptions<TModel, TEntity>(model.Search);
            var result = search.Apply(source);
            return result;
        }

        public static IDatabaseModelMapper<TModel, TEntity> ApplySearch<TModel, TEntity>(
            this IDatabaseModelMapper<TModel, TEntity> source)
            where TModel : IModel
            where TEntity : class, IDatabaseItem, new()
        {
            var search = new SearchOptions<TModel, TEntity>(source.Model.Search, source.HasAppliedSort);
            var result = search.Apply(source.AsQuerable());
            source.ApplyQuery(result);
            return source;
        }

        #endregion

        #region Mapper

        /// <summary>
        /// This creates an internal object to keep track of changes applied to a database operation to
        /// select, sort, filter, and page items and transform them into UI models
        /// </summary>
        public static IDatabaseModelMapper<TModel, TEntity> Mapper<TModel, TEntity>(this IQueryable<TEntity> source, IPagingCriteriaModel model, Func<TEntity, TModel> mapper)
            where TModel : IModel
            => new MapTypeImpl<TModel, TEntity>(source, model, mapper, null);

        public static IDatabaseModelMapper<TModel, TEntity> Mapper<TModel, TEntity>(this IQueryable<TEntity> source, IPagingCriteriaModel model, Func<List<TEntity>, Task> action, Func<TEntity, TModel> mapper)
            where TModel : IModel
            => new MapTypeImpl<TModel, TEntity>(source, model, mapper, action);

        public static IDatabaseModelMapper<TModel, TEntity> Mapper<TModel, TEntity>(this IQueryable<TEntity> source, Func<TEntity, TModel> mapper)
            where TModel : IModel
            => new MapTypeImpl<TModel, TEntity>(source, null, mapper, null);

        #endregion

        #region Mongo Methods

        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static IQueryable<TModel> ApplyPaging<TModel>(this IQueryable<TModel> source, IPagingCriteriaModel model)
            where TModel : IDatabaseItem => source.Skip(model.StartIndex()).Take(model.PageSize);

        public static IDatabaseModelMapper<TModel, TEntity> ApplySearch<TModel, TEntity>(this IDatabaseModelMapper<TModel, TEntity> source, Expression<Func<TEntity, bool>> predicate)
            where TModel : IModel
            where TEntity : IDatabaseItem
        {
            source.ApplyQuery(source.AsQuerable().Where(predicate));
            return source;
        }

        public static IDatabaseModelMapper<TModel, TEntity> ApplySearchIf<TModel, TEntity>(this IDatabaseModelMapper<TModel, TEntity> source, bool condition, Expression<Func<TEntity, bool>> predicate)
            where TModel : IModel
            where TEntity : IDatabaseItem
        {
            if (condition)
                source.ApplyQuery(source.AsQuerable().Where(predicate));
            return source;
        }

        public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, bool>> predicate)
            where TEntity : IDatabaseItem => condition ? source.Where(predicate) : source;

        #endregion

        /// <summary>
        /// Calculate the starting record index base 0
        /// To be used in LINQ/paging queries
        /// </summary>
        public static int StartIndex(this IPagingCriteriaModel model)
            => (model == null) ? 0 : (model.PageNumber - 1) * model.PageSize;

    }
}
