using System.Collections.Generic;
using CoreApplicationFilterVal_10.Domain.Common.Validation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.ValidationErrorResponse", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Common.Responses;

public class ValidationErrorResponse : ErrorResponse
{
    public IList<ValidationError> Errors { get; set; }
}
