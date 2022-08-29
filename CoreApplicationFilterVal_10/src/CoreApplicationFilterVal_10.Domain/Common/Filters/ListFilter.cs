using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreApplicationFilterVal_10.Domain.Common.Filters.Attributes;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.ListFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Filters;

public abstract class ListFilter<T>
{
    public Expression<Func<T, bool>> ToExpression()
    {
        var filterProperties = GetFilterProperties();
        if (!filterProperties.Any())
            return null;

        var expressions = new List<Expression>();
        var input = Expression.Parameter(typeof(T));

        foreach (var filterProperty in filterProperties)
        {
            var filter = filterProperty.GetCustomAttribute<FilterAttribute>();
            if (filter == null) continue;

            var filterType = filterProperty.PropertyType;
            var filterValue = filterProperty.GetValue(this);
            if (filterValue == null)
                continue;

            var split = false;
            if (filter.Split != null && filter.GetType() == typeof(InFilter))
            {
                filterType = typeof(List<string>);
                filterValue = filterValue.ToString().Split(new[] { filter.Split }, StringSplitOptions.RemoveEmptyEntries).ToList();
                split = true;
            }

            // -- get underlying types for nullable filters
            if (filterType.IsValueType && filterType.IsGenericType && filterType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var propertyType = typeof(T).GetProperty(filter.TargetField ?? filterProperty.Name)?.PropertyType;
                if (propertyType == Nullable.GetUnderlyingType(filterType))
                {
                    filterType = Nullable.GetUnderlyingType(filterType);
                    filterValue = Convert.ChangeType(filterValue, filterType);
                }
            }

            // -- convert enums into numbers
            if (filterType.IsEnum)
            {
                filterType = typeof(int);
                filterValue = Convert.ChangeType(filterValue, filterType);
            }

            // -- search filter builder
            if (filter.Split != null && filter.GetType() == typeof(SearchFilter))
            {
                var searchExpression = BuildSearchExpression(filter, input, filterType, filterValue);
                if (searchExpression != null)
                {
                    expressions.Add(searchExpression);
                    continue;
                }
            }

            // BUILD EXPRESSION

            // -- left and right
            Expression left = Expression.Property(input, filter.TargetField ?? filterProperty.Name);
            Expression right = Expression.Constant(filterValue, filterType);

            // -- type conversions
            if (left.Type.IsEnum &&
                !(right.Type.IsGenericType && right.Type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                left = Expression.Convert(left, right.Type);
            }
            else if (!split && !right.Type.IsGenericType)
                right = Expression.Convert(right, left.Type);

            // -- apply filter
            var expression = filter.ToExpression(left, right);

            // ADD TO LIST
            expressions.Add(expression);
        }

        if (expressions.Count == 0) return null;
        return BuildExpression(input, expressions);
    }

    private static Expression BuildSearchExpression(FilterAttribute filter, ParameterExpression input, Type filterType, object filterValue)
    {
        var searchFilter = (SearchFilter)filter;
        if (searchFilter.TargetField != null && searchFilter.Split != null)
        {
            var searchFields = searchFilter.TargetField
                .Split(new[] { filter.Split }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var searchChecks = new List<Expression>();
            foreach (var searchField in searchFields)
            {
                Expression searchLeft = Expression.Property(input, searchField);
                Expression searchRight = Expression.Constant(filterValue, filterType);
                searchChecks.Add(filter.ToExpression(searchLeft, searchRight));
            }

            if (searchChecks.Count > 0)
            {
                Expression searchBase = searchChecks[0];
                if (searchChecks.Count > 1)
                {
                    for (var i = 1; i < searchChecks.Count; i++)
                    {
                        searchBase = Expression.Or(searchBase, searchChecks[i]);
                    }
                }

                return searchBase;
            }
        }
        return null;
    }

    private static Expression<Func<T, bool>> BuildExpression(ParameterExpression param, List<Expression> expressions)
    {
        var parameters = new List<ParameterExpression> { param };
        var body = expressions[0];
        for (var i = 0; i < expressions.Count; i++)
        {
            if (i == 0)
                continue;
            body = Expression.AndAlso(body, expressions[i]);
        }

        var expression = Expression.Lambda<Func<T, bool>>(body, parameters);
        return expression;
    }

    private List<PropertyInfo> GetFilterProperties()
    {
        return GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetValue(this, null) != null)
            .ToList();
    }
}
