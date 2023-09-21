using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Nullref.FullStackDemo.API
{
    public static class CommonExtensions
    {
        [return: NotNull]
        public static T ThrowIfArgumentNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string callerMemberName = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException(callerMemberName);
            }
            return obj;
        }

        [return: NotNull]
        public static T ThrowIfNotFound<T>(this T item, string? objectName = null) where T : class?
        {
            if (item is Task)
            {
                throw new ArgumentException("Parameter cannot be a Task");
            }

            return item ?? throw new EntityNotFoundException(objectName);
        }

        public static string? SplitTitleCase(this string? @this)
        {
            @this = @this?.Trim();
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            if (@this!.ToUpper() == @this)
            {
                return @this;
            }

            return Regex.Replace(@this, "(\\B[A-Z])", " $1");
        }

        [return: NotNull]
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Calculate the starting record index base 0
        /// To be used in LINQ/paging queries
        /// </summary>
        public static int StartIndex(this IPagingCriteriaModel model)
            => (model == null) ? 0 : (model.PageNumber - 1) * model.PageSize;

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
