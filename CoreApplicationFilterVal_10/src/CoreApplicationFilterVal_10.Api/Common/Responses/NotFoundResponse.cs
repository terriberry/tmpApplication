using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.WebApi.REST.NotFoundResponse", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Api.Common.Responses;

public class NotFoundResponse : ErrorResponse
{
    public string Entity { get; set; }
    public string Identifier { get; set; }
}
