using System.ComponentModel.DataAnnotations;

namespace Nullref.FullStackDemo.TextToQuery
{
    public class QueryParseException : System.Exception
    {
        public QueryParseException(IDictionary<string, IEnumerable<QueryParseMessage>> errors)
        {
            Errors = errors ?? new Dictionary<string, IEnumerable<QueryParseMessage>>();
        }

        public QueryParseException(string name, QueryParseMessage error)
        {
            var list = new List<QueryParseMessage>();
            var errors = new Dictionary<string, IEnumerable<QueryParseMessage>>();
            list.Add(error);
            errors.Add(name, list);
            Errors = errors;
        }

        public QueryParseException(string message)
        {
            var list = new List<QueryParseMessage>();
            var errors = new Dictionary<string, IEnumerable<QueryParseMessage>>();
            //list.Add(error);
            //errors.Add(name, list);
            Errors = errors;
        }

        public QueryParseException(string name, string error)
            : this(name, new QueryParseMessage { Message = error }) { }

        protected QueryParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public IDictionary<string, IEnumerable<QueryParseMessage>> Errors { get; } = new Dictionary<string, IEnumerable<QueryParseMessage>>();

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

    public class QueryParseMessage
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
