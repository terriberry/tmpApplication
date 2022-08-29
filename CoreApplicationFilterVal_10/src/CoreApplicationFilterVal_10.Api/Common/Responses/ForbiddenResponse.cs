using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.ForbiddenResponse", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Common.Responses;

public class ForbiddenResponse
{
    public int StatusCode { get; set; } = 403;
    public string Message { get; set; }
    public string DisplayMessage { get; set; }
}
