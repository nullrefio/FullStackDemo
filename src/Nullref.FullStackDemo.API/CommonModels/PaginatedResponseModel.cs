namespace Nullref.FullStackDemo.API.CommonModels
{
    public interface IPaginatedResponseModel<T> : IListItemsModel<T>, IResponseModel, ISearchCriteriaModel
        where T : class
    {
        int TotalItems { get; }
        int PageNumber { get; }
        int PageSize { get; }
        int TotalPages { get; }
        string[] SortableFields { get; }
    }

    public class PaginatedResponseModel<T> : ListItemsModel<T>, IPaginatedResponseModel<T>
       where T : class
    {
        private int _totalItems = 0;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        [Required]
        public int TotalItems
        {
            get => System.Math.Max(0, _totalItems);
            set => _totalItems = System.Math.Max(0, value);
        }

        [Required]
        public int PageNumber
        {
            get => System.Math.Max(1, _pageNumber);
            set => _pageNumber = System.Math.Max(1, value);
        }

        [Required]
        public int PageSize
        {
            get => System.Math.Max(1, _pageSize);
            set => _pageSize = System.Math.Max(1, value);
        }

        /// <summary>
        /// This is a calculated value based on other properties. There is no need to set it.
        /// </summary>
        [Required]
        public int TotalPages => this.PageSize == 0 ? 0 : (int)Math.Ceiling((double)(Math.Max(this.TotalItems, 0)) / this.PageSize);

        [Required]
        public string[] SortableFields { get; set; } = Array.Empty<string>();
    }
}
