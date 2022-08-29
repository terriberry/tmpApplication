using System;
using System.Linq.Expressions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.ContainsFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Filters.Attributes;

public class ContainsFilter : FilterAttribute
{
    public override Expression ToExpression(Expression left, Expression right)
    {
        var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        return Expression.Call(left, containsMethod, right);
    }
}