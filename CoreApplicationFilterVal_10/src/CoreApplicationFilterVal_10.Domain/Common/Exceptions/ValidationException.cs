using System;
using System.Collections.Generic;
using System.Linq;
using CoreApplicationFilterVal_10.Domain.Common.Validation;
using FluentValidation.Results;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Common.ValidationException", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public IList<ValidationError> Errors { get; }

    public ValidationException(string message, IList<ValidationError> errors) : base(message)
    {
        Errors = errors;
    }

    public ValidationException(string message, ValidationError error) : base(message)
    {
        Errors = new List<ValidationError> { error };
    }

    public ValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message)
    {
        Errors = errors.Select(e => new ValidationError
        {
            ErrorMessage = e.ErrorMessage,
            DisplayMessage = e.ErrorMessage,
            ErrorType = ValidationErrorType.InvalidOption,
            Field = e.PropertyName
        }).ToList();
    }


    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception inner) : base(message, inner)
    {
    }
}
