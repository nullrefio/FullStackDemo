namespace Nullref.FullStackDemo.CommonModels.Attributes
{
    public static class ValidationConstants
    {
        // "reasonable" min/max date range for default/blanket validations
        // specific properties may be more strict
        public const string MinDate = "2000-01-01";
        public const string MaxDate = "2099-12-31";
        public const string MinDateTime = "2000-01-01T00:00:00.0000000";
        public const string MaxDateTime = "2099-12-31T00:00:00.0000000";
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LocalDateRangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        public LocalDateRangeAttribute()
            : base(typeof(NodaTime.LocalDate), ValidationConstants.MinDate, ValidationConstants.MaxDate)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var v = (object?)value;
            if (value is DateTime time)
                v = time.ToLocalDate();

            if (v != null && v is not NodaTime.LocalDate && !(v is NodaTime.LocalDate?))
                return new ValidationResult($"Invalid property type '{validationContext.ObjectType.Name}.{validationContext.DisplayName}'");
            return base.IsValid(v, validationContext);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LocalDateTimeRangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        public LocalDateTimeRangeAttribute()
            : base(typeof(NodaTime.LocalDateTime), ValidationConstants.MinDateTime, ValidationConstants.MaxDateTime)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var v = (object?)value;
            if (value is DateTime time)
                v = time.ToLocalDateTime();

            if (v != null && v is not NodaTime.LocalDateTime && !(v is NodaTime.LocalDateTime?))
                return new ValidationResult($"Invalid property type '{validationContext.ObjectType.Name}.{validationContext.DisplayName}'");
            return base.IsValid(v, validationContext);
        }
    }
}
