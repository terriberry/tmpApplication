using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.ValidationErrorType", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Validation;

public enum ValidationErrorType
{
    NotNull,
    NotEmpty,
    MinLength,
    MaxLength,
    MaxValue,
    MinValue,
    InvalidOption,
    InvalidReference,
    RuleViolation
}