using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.DomainServiceInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Contracts;

public interface IDomainService : IService
{

}
