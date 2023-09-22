namespace Nullref.FullStackDemo.CommonModels
{
    public interface IListItemsModel<T> : IResponseModel, ISearchCriteriaModel
        where T : class
    {
        List<T> Items { get; }
    }

    public class ListItemsModel<T> : IListItemsModel<T>
        where T : class
    {
        [Required]
        public List<T> Items { get; set; } = new();

        [MaxLength(100)]
        public string? Search { get; set; }

        [MaxLength(100)]
        public string? Order { get; set; }
    }
}
