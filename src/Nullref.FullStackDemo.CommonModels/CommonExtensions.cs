using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Nullref.FullStackDemo.CommonModels
{
    public static class CommonExtensions
    {
        [return: NotNull]
        public static T ThrowIfArgumentNull<T>([NotNull] this T obj, [CallerArgumentExpression("obj")] string callerMemberName = "")
        {
            if (obj == null)
            {
                throw new ArgumentNullException(callerMemberName);
            }
            return obj;
        }

        [return: NotNull]
        public static T ThrowIfNotFound<T>(this T item, string? objectName = null)
            where T : class?
            => item ?? throw new EntityNotFoundException(objectName);

        public static string? SplitTitleCase(this string? @this)
        {
            @this = @this?.Trim();
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            if (@this!.ToUpper() == @this)
            {
                return @this;
            }

            return Regex.Replace(@this, "(\\B[A-Z])", " $1");
        }

        [return: NotNull]
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
