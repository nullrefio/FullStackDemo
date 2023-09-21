namespace Nullref.FullStackDemo.CommonModels
{
    public interface IResponseModel { }

    public class IdResponseModel : IResponseModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
