using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using CoreApplicationFilterVal_10.Domain.Common.Services;
using CoreApplicationFilterVal_10.Domain.Contracts.Repositories;
using CoreApplicationFilterVal_10.Domain.Contracts.Services;
using CoreApplicationFilterVal_10.Domain.Models.Entities.Owners;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("TD.Domain.Services.Services", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Services;

public class OwnerService : ReadWriteService<int, Owner, OwnerVM, OwnerDTO, OwnerDTO>, IOwnerService
{
    public OwnerService(IOwnerReadWriteRepository repository, IMapper mapper, IValidator<PagedFilter> pagedFilterValidator, IValidator<OwnerDTO> createValidator, IValidator<OwnerDTO> updateValidator)
            : base(repository, mapper, pagedFilterValidator, createValidator, updateValidator)
    {
    }
    public override async ValueTask<PagedList<OwnerVM>> PagedList(PagedFilter pagedFilter, OwnerFilter ownerFilter) 
    {

    }
}
