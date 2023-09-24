namespace Nullref.FullStackDemo.CommonModels
{
    public interface ISearchCriteriaModel : IModel
    {
        string? Order { get; set; }
        string? Search { get; set; }
    }

    public class SearchCriteriaModel : ISearchCriteriaModel
    {
        [MaxLength(100)]
        public string? Order { get; set; }

        [MaxLength(100)]
        public string? Search { get; set; }
    }

}
