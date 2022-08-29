using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.ErrorResponse", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Common.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; } = 400;
    public string Message { get; set; }
    public string DisplayMessage { get; set; }
}
