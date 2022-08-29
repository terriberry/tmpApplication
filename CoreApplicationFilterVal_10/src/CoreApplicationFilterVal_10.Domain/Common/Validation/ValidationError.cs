using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.ValidationError", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Validation;

public class ValidationError
{
    public ValidationErrorType ErrorType { get; set; }
    public string Field { get; set; }
    public string Value { get; set; }
    public string ErrorMessage { get; set; }
    public string DisplayMessage { get; set; }


    public static ValidationError InvalidOption(string field, string value)
    {
        return new ValidationError
        {
            ErrorType = ValidationErrorType.InvalidOption,
            Field = field,
            Value = value,
            ErrorMessage = $"'{value}' not valid option for '{field}",
            DisplayMessage = $"Incorrect value for supplied for {field}"
        };
    }

    public static ValidationError InvalidReference(string field, string value)
    {
        return new ValidationError
        {
            ErrorType = ValidationErrorType.InvalidReference,
            Field = field,
            Value = value,
            ErrorMessage = $"Could not resolve reference '{field}' with id '{value}'",
            DisplayMessage = $"Could not find '{field}' with id '{value}'"
        };
    }
}