using System.Reflection;

namespace Nullref.FullStackDemo.TextToQuery
{
    internal static class Extensions
    {
        /// <summary>
        /// Case insensitive match
        /// </summary>
        public static bool Match(this string? str, string? compare)
        {
            if (str == null && compare == null) return true;
            if (str == string.Empty && compare == string.Empty) return true;
            return str?.ToLower() == compare?.ToLower();
        }

        public static string? ToCamelCase(this string? str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            return str;
        }

        /// <summary>
        /// Parse string in array blocking text in quotes.
        /// If a text block is enclosed in quotes it will be returned as a single entry.
        /// Outside of quotes, text will be broken on spaces.
        /// </summary>
        public static string[] ParseWithQuotes(string baseString)
        {
            //Replace double quotes with a replacement string for later processing
            var r = "~!~!";
            baseString = (baseString + string.Empty).Replace("\"\"", r);

            var aux = baseString.Split('"');
            var tokens = new List<string>();
            for (var i = 0; i < aux.Length; ++i)
                if (i % 2 == 0)
                    tokens.AddRange(aux[i].Split(' '));
                else
                    tokens.Add(aux[i]);

            // Replace the original double quote with a single quote
            return tokens
                .Where(x => x != string.Empty)
                .Select(x => x.Replace(r, "\""))
                .Where(x => x != "\"") //remove any quote only entries
                .ToArray();
        }

    }

    public static class TypeInfoAllMemberExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredProperties);

        private static IEnumerable<T> GetAll<T>(TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
        {
            var t = typeInfo;
            while (t != null)
            {
                foreach (var tloop in accessor(t))
                    yield return tloop;
                t = t.BaseType?.GetTypeInfo();
            }
        }
    }
}
