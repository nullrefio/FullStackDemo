using System.ComponentModel.DataAnnotations;

namespace Nullref.FullStackDemo.TextToQuery
{
    /// <summary>
    /// This object takes a URL order by string like "Field1|Field2,desc|Field3,Asc"
    /// </summary>
    public class SortOptions<TModel, TEntity> : SortOptions, IValidatableObject
    {
        private List<string> _applied = new();

        public SortOptions(string orderBy, bool hasAppliedSort = false)
        {
            this.OrderBy = (orderBy ?? string.Empty).Split('|', StringSplitOptions.RemoveEmptyEntries);
            this.HasAppliedSort = hasAppliedSort;
        }

        public string[] OrderBy { get; private set; }

        public bool HasAppliedSort { get; private set; }

        // ASP.NET Core calls this validate incoming parameters
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SortOptionsProcessor<TModel, TEntity>(OrderBy, HasAppliedSort);

            var validTerms = processor.GetValidTerms().Select(x => x.Field);

            var invalidTerms = processor.GetAllTerms().Select(x => x.Field)
                .Except(validTerms, StringComparer.OrdinalIgnoreCase);

            foreach (var term in invalidTerms)
            {
                yield return new ValidationResult(
                    $"Invalid sort term '{term}'.",
                    new[] { nameof(OrderBy) });
            }
        }

        // The service code will call this to apply these sort options to a database query
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            _applied.Clear();
            var processor = new SortOptionsProcessor<TModel, TEntity>(OrderBy, this.HasAppliedSort);
            var result = processor.Apply(query);
            this.OrderBy = processor.OrderBy;
            this.HasAppliedSort = processor.HasAppliedSort;
            _applied = processor.AppliedSorts.ToList();
            return result;
        }

        public IEnumerable<string> AppliedSorts => _applied.ToList();
    }

    public abstract class SortOptions
    {
        public static string CreateSort(string field, bool ascending = true)
            => field + "," + (ascending ? "asc" : "desc");
    }
}
