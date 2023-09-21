namespace Nullref.FullStackDemo.API.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortableAttribute : System.Attribute
    {
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
