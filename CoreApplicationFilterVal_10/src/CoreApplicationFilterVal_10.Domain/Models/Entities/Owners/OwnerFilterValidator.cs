using System;
using System.Linq;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Entities.Filters.Validation.Fluent.EntityFilterValidation", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;

[IntentManaged(Mode.Merge, Signature = Mode.Fully)]
public class OwnerFilterValidator : AbstractValidator<OwnerFilter>
{
    [IntentManaged(Mode.Fully)]
    public OwnerFilterValidator()
    {
        RuleFor(v => v.Name)
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Name Must not contain any special characters.")
            .Must(x => !x.Any(char.IsDigit)).WithMessage("Name must not contain any numbers.");
        RuleFor(v => v.Age)
            .LessThanOrEqualTo(150);
    }

}