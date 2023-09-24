namespace Nullref.FullStackDemo.CommonModels
{
    public interface IResponseModel : IModel { }

    public class IdResponseModel : IResponseModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
