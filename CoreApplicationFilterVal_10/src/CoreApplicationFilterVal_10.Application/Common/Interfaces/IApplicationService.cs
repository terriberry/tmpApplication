using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Application.Services.ApplicationServiceInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Application.Common.Interfaces;

public interface IApplicationService : IService
{

}
