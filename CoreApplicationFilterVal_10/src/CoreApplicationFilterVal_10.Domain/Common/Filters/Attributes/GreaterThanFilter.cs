using System.Linq.Expressions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.GreaterThanFilter", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Filters.Attributes;

public class GreaterThanFilter : FilterAttribute
{
    public override Expression ToExpression(Expression left, Expression right)
    {
        var expression = Expression.GreaterThan(left, right);

        var nullCheck = Expression.Equal(left, Expression.Constant(null, typeof(object)));
        if (IncludeNulls)
            expression = Expression.Or(
                nullCheck,
                expression
            );

        return expression;
    }
}