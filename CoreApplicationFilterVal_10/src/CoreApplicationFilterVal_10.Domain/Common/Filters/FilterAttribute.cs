using System;
using System.Linq.Expressions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Entities.Filters.FilterAttribute", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Filters;

public abstract class FilterAttribute : Attribute
{
    public string TargetField { get; set; }
    public bool IncludeNulls { get; set; } = false;
    public string Split { get; set; }
    public abstract Expression ToExpression(Expression left, Expression right);
}