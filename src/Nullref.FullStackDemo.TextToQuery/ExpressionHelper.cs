using System.Linq.Expressions;
using System.Reflection;

namespace Nullref.FullStackDemo.TextToQuery
{
    public static class ExpressionHelper
    {
        private static readonly MethodInfo LambdaMethod = typeof(Expression)
            .GetMethods()
            .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2);

        private static readonly MethodInfo[] QueryableMethods = typeof(Queryable)
            .GetMethods()
            .ToArray();

        private static MethodInfo GetLambdaFuncBuilder(Type source, Type dest)
        {
            var predicateType = typeof(Func<,>).MakeGenericType(source, dest);
            return LambdaMethod.MakeGenericMethod(predicateType);
        }

        public static LambdaExpression? GetLambda<TSource, TDest>(ParameterExpression obj, Expression arg)
            => GetLambda(typeof(TSource), typeof(TDest), obj, arg);

        public static LambdaExpression GetLambda(Type source, Type dest, ParameterExpression obj, Expression arg)
        {
            var lambdaBuilder = GetLambdaFuncBuilder(source, dest);
            if (lambdaBuilder == null) throw new QueryParseException("Item not found");
            var result = (LambdaExpression?)lambdaBuilder.Invoke(null, new object[] { arg, new[] { obj } });
            if (result == null) throw new QueryParseException("Item not found");
            return result;
        }

        public static IQueryable<T>? CallWhere<T>(IQueryable<T> query, LambdaExpression predicate)
        {
            var whereMethodBuilder = QueryableMethods
                .First(x => x.Name == "Where" && x.GetParameters().Length == 2)
                .MakeGenericMethod(new[] { typeof(T) });
            if (whereMethodBuilder == null) return default;

            return (IQueryable<T>?)whereMethodBuilder
                .Invoke(null, new object[] { query, predicate }) ?? default;
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source,
                                       string? propertyName,
                                       bool useThenBy,
                                       bool descending)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (String.IsNullOrEmpty(propertyName)) return source;

            // Create a parameter to pass into the Lambda expression
            //(Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(T), "Entity");

            //  create the selector part, but support child properties (it works without . too)
            String[] childProperties = propertyName.Split('.');
            MemberExpression property = Expression.Property(parameter, childProperties[0]);
            for (int i = 1; i < childProperties.Length; i++)
            {
                property = Expression.Property(property, childProperties[i]);
            }

            var selector = Expression.Lambda(property, parameter);
            var methodName = "OrderBy";
            if (useThenBy) methodName = "ThenBy";
            if (descending) methodName += "Descending";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
                                            new Type[] { source.ElementType, property.Type },
                                            source.Expression, Expression.Quote(selector));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
