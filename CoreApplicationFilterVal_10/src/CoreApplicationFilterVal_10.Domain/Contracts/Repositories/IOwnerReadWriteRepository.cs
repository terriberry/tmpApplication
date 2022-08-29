using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Contracts.Repositories.EntityReadWriteRepositoryInterfaces", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Contracts.Repositories;

public interface IOwnerReadWriteRepository : IReadWriteRepository<int, Owner>
{
}
