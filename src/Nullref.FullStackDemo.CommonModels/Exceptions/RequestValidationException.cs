namespace Nullref.FullStackDemo.CommonModels.Exceptions
{
    /// <summary>
    /// Used to provide a general message explaining why a request cannot be processed.
    /// </summary>
    [Serializable]
    public class RequestValidationException : Exception
    {
        public RequestValidationException(IDictionary<string, IEnumerable<FieldErrorModel>> errors)
        {
            Errors = errors ?? new Dictionary<string, IEnumerable<FieldErrorModel>>();
        }

        public RequestValidationException(string name, FieldErrorModel error)
        {
            var list = new List<FieldErrorModel>();
            var errors = new Dictionary<string, IEnumerable<FieldErrorModel>>();
            list.Add(error);
            errors.Add(name, list);
            Errors = errors;
        }

        public RequestValidationException(string name, string error)
            : this(name, new FieldErrorModel { Message = error }) { }

        protected RequestValidationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public IDictionary<string, IEnumerable<FieldErrorModel>> Errors { get; } = new Dictionary<string, IEnumerable<FieldErrorModel>>();

        public override string ToString()
        {
            var index = 1;
            var result = this.Message +
                System.Environment.NewLine +
                $"Error List ({this.Errors.Count})" + System.Environment.NewLine +
                string.Join(System.Environment.NewLine, this.Errors.Values.SelectMany(x => x.Select(z => $"{index++}. {z?.Message}"))) + System.Environment.NewLine +
                base.ToString();
            return result;
        }
    }

    public class FieldErrorModel
    {
        [Required]
        public string? Message { get; set; }

        [Required]
        public int ErrorNumber { get; set; } = 0;

        [MaxLength(2000)]
        public string? AttributeType { get; set; }

        public Guid? AttributeId { get; set; }
    }

}
