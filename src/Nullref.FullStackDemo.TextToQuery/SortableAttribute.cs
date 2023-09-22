namespace Nullref.FullStackDemo.TextToQuery
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortableAttribute : System.Attribute
    {
        /// <summary>
        /// Determines the database field to which this model property is mapped
        /// </summary>
        /// <remarks>Leave null if there is no difference in names</remarks>
        public string? MappedField { get; set; }

        public string? MappedEntity { get; set; }

        /// <summary>
        /// This can be used to define a custom property name that does NOT match the object property name
        /// This is optional and if not specified will pull the property name to which it is attached
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// Determines if the property is the default sort when no other explcit sort is defined
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// If the IsDefault property is true, then this is used for the sort direction
        /// </summary>
        public bool IsDefaultAsc { get; set; } = true;
    }
}
