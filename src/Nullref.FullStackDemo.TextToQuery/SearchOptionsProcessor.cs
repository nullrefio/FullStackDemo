using System.Linq.Expressions;
using System.Reflection;

namespace Nullref.FullStackDemo.TextToQuery
{
    public class SearchOptionsProcessor<TModel, TEntity>
    {
        private readonly string[] _searchWords;
        private readonly bool _lowerProperty = false;

        public SearchOptionsProcessor(string[] searchWords, bool lowerProperty = false)
        {
            _searchWords = searchWords;
            _lowerProperty = lowerProperty;
        }

        // Unable to figure out how to use the contain expression for UserDefinedFieldValueList
        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var props = typeof(TModel).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(SearchableAttribute)));
            if (!_searchWords.Any() || !props.Any())
                return query;

            //Convert all strings to ConstantExpressions
            var searchTerms = _searchWords.Select(Expression.Constant).ToList();
            if (_lowerProperty)
                searchTerms = _searchWords.Select(match => Expression.Constant(match.ToLower())).ToList();

            var dbObj = Expression.Parameter(typeof(TEntity), string.Empty);
            var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            var lowerMethodInfo = typeof(string).GetMethod("ToLower", new System.Type[0])!;
            Expression? masterExpression = null;

            searchTerms.ForEach(searchTerm =>
            {
                foreach (var p in props)
                {
                    var searchAttrs = p.GetCustomAttributes<SearchableAttribute>();
                    foreach (var searchAttr in searchAttrs)
                    {
                        var dbFieldName = searchAttr.MappedField ?? p.Name;
                        MemberExpression? dbaProperty = null;
                        if (string.IsNullOrEmpty(searchAttr.MappedEntity))
                        {
                            // Normal strings on primary object
                            dbaProperty = Expression.PropertyOrField(dbObj, dbFieldName);
                        }
                        else
                        {
                            // Property on child object
                            dbaProperty = Expression.PropertyOrField(dbObj, searchAttr.MappedEntity);
                            //Normal property of parent object
                            dbaProperty = Expression.PropertyOrField(dbaProperty, dbFieldName);
                        }

                        Expression? expressionCalls = null;
                        if (_lowerProperty)
                        {
                            Expression lowerCall = Expression.Call(dbaProperty, lowerMethodInfo);
                            expressionCalls = Expression.Call(lowerCall, containsMethodInfo, searchTerm);
                        }
                        else
                        {
                            expressionCalls = Expression.Call(dbaProperty, containsMethodInfo, searchTerm);
                        }

                        //nullCheck = Expression.NotEqual(dbaProperty, Expression.Constant(null));
                        //expressionCalls = Expression.AndAlso(nullCheck, expressionCalls);

                        if (masterExpression != null)
                            expressionCalls = Expression.OrElse(masterExpression, expressionCalls);
                        masterExpression = expressionCalls;
                    }
                }
            });

            if (masterExpression == null)
                return query;
            var lambdaExpression = ExpressionHelper.GetLambda<TEntity, bool>(dbObj, masterExpression);
            if (lambdaExpression == null) return query;
            return ExpressionHelper.CallWhere(query, lambdaExpression) ?? query;
        }
    }
}
