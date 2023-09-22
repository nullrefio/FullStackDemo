namespace Nullref.FullStackDemo.TextToQuery
{
    public class SearchOptions<TModel, TEntity>
    {
        public SearchOptions(string search, bool lowerProperty = false)
        {
            this.SearchWords = Extensions.ParseWithQuotes(search);
            this.LowerProperty = lowerProperty;
        }

        public string[] SearchWords { get; }
        public bool LowerProperty { get; }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var processor = new SearchOptionsProcessor<TModel, TEntity>(SearchWords, LowerProperty);
            return processor.Apply(query);
        }
    }
}
