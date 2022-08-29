using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.ServiceContracts", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Contracts.Services;

public interface IOwnerService : IReadWriteService<int, Owner, OwnerVM, OwnerDTO, OwnerDTO>, IDomainService
{
}
