namespace Nullref.FullStackDemo.CommonModels.Exceptions
{
    [Serializable]
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string? message) : base(message) { }
    }
}
