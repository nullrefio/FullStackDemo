namespace Nullref.FullStackDemo.API.CommonModels
{
    public interface IResponseModel { }

    public class IdResponseModel : IResponseModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
