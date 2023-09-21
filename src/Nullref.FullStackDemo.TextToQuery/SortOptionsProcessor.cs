using System.Reflection;

namespace Nullref.FullStackDemo.TextToQuery
{
    public class SortOptionsProcessor<TModel, TEntity>
    {
        private List<string> _applied = new();

        public string[] OrderBy { get; private set; }

        public bool HasAppliedSort { get; private set; }

        public IEnumerable<string> AppliedSorts => _applied.ToList();

        public SortOptionsProcessor(string[] orderBy, bool hasAppliedSort)
        {
            OrderBy = orderBy;
            this.HasAppliedSort = hasAppliedSort;
        }

        /// <summary>
        /// The OrderBy terms are strings like "Field1" or "Field1,asc" or "Field1,desc"
        /// </summary>
        public IEnumerable<SortTerm> GetAllTerms()
        {
            if (OrderBy == null) yield break;
            foreach (var term in OrderBy)
            {
                if (string.IsNullOrEmpty(term)) continue;

                var tokens = term
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

                if (tokens.Length == 0)
                    continue;

                var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                var arr = tokens[0].Split('.');
                var field = arr[0];
                string? entity = null;
                if (arr.Length == 2)
                {
                    entity = arr[0];
                    field = arr[1];
                }

                yield return new SortTerm
                {
                    Field = field,
                    Descending = descending,
                    MappedEntity = entity,
                };
            }
        }

        public IEnumerable<SortTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().ToArray();
            if (!queryTerms.Any()) yield break;

            var declaredTerms = GetTermsFromModel();

            foreach (var term in queryTerms)
            {
                //Check for aliased property name
                var declaredTerm = declaredTerms.SingleOrDefault(x => x.GetField().Match(term.GetField())) ?? declaredTerms.SingleOrDefault(x => x.PropertyName?.Match(term.GetField()) ?? false);
                declaredTerm ??= declaredTerms.SingleOrDefault(x => !x.PropertyName.Match(term.GetField()) && x.Field.Match(term.GetField()));

                if (declaredTerm == null)
                {
                    var errors = new Dictionary<string, IEnumerable<QueryParseMessage>>();
                    var k = term.GetField();
                    if (k != null)
                        errors.Add(k, new List<QueryParseMessage> { new QueryParseMessage { Message = $"Sort field '{term.GetField()}' not found." } });
                    throw new QueryParseException(errors);
                }
                yield return new SortTerm
                {
                    Field = declaredTerm.Field,
                    MappedField = declaredTerm.MappedField,
                    Descending = term.Descending,
                    IsDefault = declaredTerm.IsDefault,
                    MappedEntity = declaredTerm.MappedEntity,
                };
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            _applied.Clear();
            var terms = GetValidTerms().ToArray();
            if (!terms.Any())
            {
                terms = GetTermsFromModel().Where(t => t.IsDefault).ToArray();
                //If usin the default sort then set it so can be returned to UI
                if (!OrderBy.Any())
                {
                    OrderBy = terms.Select(x => (x.MappedField ?? x.Field).ToCamelCase() + "," + (x.Descending ? "desc" : "asc")).ToArray();
                }
            }
            if (!terms.Any()) return query;

            var modifiedQuery = query;
            foreach (var term in terms)
            {
                modifiedQuery = modifiedQuery.SortBy(term.GetField(), HasAppliedSort, term.Descending);
                HasAppliedSort = true;
            }
            _applied.AddRange(terms.Select(x => x.ToString()));
            return modifiedQuery;
        }

        private static IEnumerable<SortTerm> GetTermsFromModel()
        {
            var props = typeof(TModel).GetTypeInfo()
                .GetAllProperties()
                .Where(p => p.GetCustomAttributes<SortableAttribute>().Any());

            foreach (var p in props)
            {
                foreach (var p2 in p.GetCustomAttributes<SortableAttribute>())
                {
                    yield return new SortTerm
                    {
                        PropertyName = p2.PropertyName,
                        Field = p.Name,
                        MappedField = p2.MappedField,
                        IsDefault = p2.IsDefault,
                        Descending = p2.IsDefault ? !p2.IsDefaultAsc : false,
                        MappedEntity = p2.MappedEntity,
                    };
                }
            }
        }
    }
}
