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
    private readonly IValidator<OwnerFilter> _filterValidator;

    public OwnerService(IOwnerReadWriteRepository repository, IMapper mapper, IValidator<PagedFilter> pagedFilterValidator, IValidator<OwnerDTO> createValidator, IValidator<OwnerDTO> updateValidator, IValidator<OwnerFilter> filterValidator)
            : base(repository, mapper, pagedFilterValidator, createValidator, updateValidator)
    {
        _filterValidator = filterValidator;
    }
    public override async ValueTask<PagedList<TViewModel>> PagedList(PagedFilter pagedFilter, OwnerFilter ownerFilter) 
    {
        var validationResult = await _filterValidator.ValidateAsync(ownerFilter);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("One or more validation errors", validationResult.Errors);
        }
        return await base.PagedList(pagedFilter, ownerFilter);
    }
}
