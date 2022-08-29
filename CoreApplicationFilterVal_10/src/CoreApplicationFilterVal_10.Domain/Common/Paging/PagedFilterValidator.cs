using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using Microsoft.Extensions.Configuration;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Services.Validation.Fluent.PagedFilterValidator", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Paging;

[IntentManaged(Mode.Merge, Signature = Mode.Fully)]
public class PagedFilterValidator : AbstractValidator<PagedFilter>
{
    [IntentManaged(Mode.Fully)]
    public PagedFilterValidator(IConfiguration configuration)
    {
        var maxRecords = configuration.GetValue<int>("Pagination:MaxRecords");

        RuleFor(p => p.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(maxRecords);

        RuleFor(p => p.PageNo)
            .GreaterThanOrEqualTo(1);
    }
}
