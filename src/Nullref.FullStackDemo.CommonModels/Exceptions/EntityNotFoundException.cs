namespace Nullref.FullStackDemo.CommonModels.Exceptions
{
    /// <summary>
    /// Used to indicate that an entity lookup failed.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        private const string DefaultMessage = "The item was not found.";

        public EntityNotFoundException()
            : base(DefaultMessage) { }

        public EntityNotFoundException(string? objectName)
            : base(string.IsNullOrEmpty(objectName) ? DefaultMessage : $"{DefaultMessage} '{objectName.SplitTitleCase()}'") { }

        protected EntityNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
