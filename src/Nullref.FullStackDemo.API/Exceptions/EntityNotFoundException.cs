using System.Runtime.Serialization;

namespace Nullref.FullStackDemo.API.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string DefaultMessage = "The item was not found.";

        public EntityNotFoundException()
            : base("The item was not found.")
        {
        }

        public EntityNotFoundException(string? objectName)
            : base(string.IsNullOrEmpty(objectName) ? "The item was not found." : ("The item was not found. '" + objectName.SplitTitleCase() + "'"))
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
