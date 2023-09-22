namespace Nullref.FullStackDemo.TextToQuery
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SearchableAttribute : System.Attribute
    {
        /// <summary>
        /// Determines the database field to which this model property is mapped
        /// </summary>
        /// <remarks>Leave null if there is no difference in names</remarks>
        public string? MappedField { get; set; }

        public string? MappedEntity { get; set; }
    }
}
