namespace Nullref.FullStackDemo.CommonModels.Exceptions
{
    /// <summary>
    /// Used to indicate that an entity exists.
    /// </summary>
    [Serializable]
    public class EntityExistsException : Exception
    {
        private const string DefaultMessage = "The item exists.";

        public EntityExistsException()
            : base(DefaultMessage) { }

        public EntityExistsException(string? objectName)
            : base(string.IsNullOrEmpty(objectName) ? DefaultMessage : $"{DefaultMessage} '{objectName.SplitTitleCase()}'") { }

        protected EntityExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
