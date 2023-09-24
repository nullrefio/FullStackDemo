using NodaTime;

namespace Nullref.FullStackDemo.CommonModels
{
    /// <summary>
    /// Extensions to help consistently convert values from <see cref="DateTime"/> to NodaTime types and back. 
    /// </summary>
    public static class NodaTimeExtensions
    {
        public static DateTime ToDateTime(this LocalDate value)
            => DateTime.SpecifyKind(value.ToDateTimeUnspecified(), DateTimeKind.Utc);

        public static LocalDate ToLocalDate(this DateTime value)
            => LocalDate.FromDateTime(value);

        public static LocalDateTime ToLocalDateTime(this DateTime value)
            => LocalDateTime.FromDateTime(value);

        public static DateTime ToUtcDate(this DateTime value)
            => DateTime.SpecifyKind(value.ToLocalDate().ToDateTimeUnspecified(), DateTimeKind.Utc);

        public static DateTime ToDateTime(this LocalDateTime value)
            => DateTime.SpecifyKind(value.ToDateTimeUnspecified(), DateTimeKind.Utc);
    }
}
