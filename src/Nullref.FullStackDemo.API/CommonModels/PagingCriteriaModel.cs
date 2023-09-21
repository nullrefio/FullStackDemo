namespace Nullref.FullStackDemo.API.CommonModels
{
    public interface IPagingCriteriaModel : ISearchCriteriaModel
    {
        int PageNumber { get; }
        int PageSize { get; }
    }

    public class PagingCriteriaModel : SearchCriteriaModel, IPagingCriteriaModel
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        [Required]
        [DefaultValue(1)]
        public int PageNumber
        {
            get => System.Math.Max(1, _pageNumber);
            set => _pageNumber = System.Math.Max(1, value);
        }

        [Required]
        [DefaultValue(10)]
        public int PageSize
        {
            get => System.Math.Max(1, _pageSize);
            set => _pageSize = System.Math.Max(1, value);
        }
    }

}
