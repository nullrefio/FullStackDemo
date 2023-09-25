namespace Nullref.FullStackDemo.TextToQuery
{
    internal class SortTerm
    {
        public string? Field { get; set; }
        public string? MappedField { get; set; }
        public string? MappedEntity { get; set; }
        public bool Descending { get; set; }
        public bool IsDefault { get; set; }

        /// <summary>
        /// Optional value that specifies the property name the sort attribute was attached to
        /// If null then the object's property name is used
        /// </summary>
        public string? PropertyName { get; set; }

        public virtual string? GetField()
        {
            if (string.IsNullOrEmpty(this.MappedEntity))
                return this.MappedField ?? this.Field;
            else
                return this.MappedEntity + "." + (this.MappedField ?? this.Field);
        }

        public override string ToString() => this.Field + (this.Descending ? ",Desc" : ",Asc");
    }
}
