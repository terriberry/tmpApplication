using System;
using System.Linq;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.Validation.Fluent.DomainDtoValidation", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;

[IntentManaged(Mode.Merge, Signature = Mode.Fully)]
public class OwnerVMValidator : AbstractValidator<OwnerVM>
{
    [IntentManaged(Mode.Fully)]
    public OwnerVMValidator()
    {
        RuleFor(v => v.Name)
            .NotNull().WithMessage("Name must not be null.")
            .NotEmpty().WithMessage("Name must not be empty.");
    }

}
