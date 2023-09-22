namespace Nullref.FullStackDemo.CommonModels.Exceptions
{
    [Serializable]
    public class GenericException : Exception
    {
        public GenericException(string message) : base(message) { }

        protected GenericException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
