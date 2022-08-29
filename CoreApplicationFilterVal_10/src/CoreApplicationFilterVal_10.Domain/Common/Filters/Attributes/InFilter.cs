using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreApplicationFilterVal_10.Domain.Common.Exceptions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.InFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Filters.Attributes;

public class InFilter : FilterAttribute
{
    public override Expression ToExpression(Expression left, Expression right)
    {
        if (right.Type.IsGenericType && right.Type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var typeDefinition = right.Type.GetGenericTypeDefinition();
            //if (typeDefinition.GetGenericArguments().FirstOrDefault() == typeof(string))
            //{
            //    // Method info for List<string>.Contains(code).
            //    var containsMethod = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });
            //    return Expression.Call(right, containsMethod, left);
            //}
            var collectionType = right.Type;
            var itemType = collectionType.GenericTypeArguments.FirstOrDefault();

            var containsMethod = collectionType.GetMethod("Contains", new Type[] { itemType });
            return Expression.Call(right, containsMethod, left);
        }

        throw new ValidationException("Could not resolve filter expression");
    }
}