using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Domain.Common.Exceptions;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using CoreApplicationFilterVal_10.Domain.Contracts.Services;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using ValidationException = CoreApplicationFilterVal_10.Domain.Common.Exceptions.ValidationException;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.CRUD.ReadWriteService", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Services;

public abstract class ReadWriteService<TKey, TEntity, TViewModel, TCreateModel, TUpdateModel> : ReadOnlyService<TKey, TEntity, TViewModel>, IReadWriteService<TKey, TEntity, TViewModel, TCreateModel, TUpdateModel>
    where TEntity : class, IEntity
    where TViewModel : class
    where TCreateModel : class
    where TUpdateModel : class
{

    private readonly IMapper _mapper;
    private readonly IReadWriteRepository<TKey, TEntity> _repository;
    private readonly IValidator<TCreateModel> _createValidator;
    private readonly IValidator<TUpdateModel> _updateValidator;

    protected ReadWriteService(IReadWriteRepository<TKey, TEntity> repository, IMapper mapper,
        IValidator<PagedFilter> pagedFilterValidator, IValidator<TCreateModel> createValidator, IValidator<TUpdateModel> updateValidator) : base(repository, mapper, pagedFilterValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async ValueTask<TViewModel> Create(TCreateModel createModel)
    {
        var validationResult = await _createValidator.ValidateAsync(createModel);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("One or more validation errors", validationResult.Errors);
        }
        var model = _mapper.Map<TEntity>(createModel);
        model = await _repository.Create(model);
        return _mapper.Map<TViewModel>(model);
    }

    public async ValueTask<IEnumerable<TViewModel>> Create(IEnumerable<TCreateModel> createModels)
    {
        var entities = new List<TEntity>();
        foreach (var createModel in createModels)
        {
            var validationResult = await _createValidator.ValidateAsync(createModel);
            if (!validationResult.IsValid)
                throw new ValidationException("One or more validation errors", validationResult.Errors);

            var model = _mapper.Map<TEntity>(createModel);
            entities.Add(model);
        }
        var createResponse = await _repository.Create(entities);
        return createResponse.Select(c => _mapper.Map<TViewModel>(c));
    }

    public async ValueTask<TViewModel> Update(TKey key, TUpdateModel updateModel)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateModel);
        if (!validationResult.IsValid)
            throw new ValidationException("One or more validation errors", validationResult.Errors);

        var model = await _repository.Get(key);
        if (model == null)
            throw new NotFoundException(nameof(TEntity), $"{key}");

        model = _mapper.Map(updateModel, model);
        model = await _repository.Update(key, model);

        return _mapper.Map<TViewModel>(model);
    }

    public async Task Delete(TKey key)
    {
        var deleteSuccessful = await _repository.Delete(key);
        if (!deleteSuccessful)
        {
            throw new NotFoundException(nameof(TEntity), $"{key}");
        }
    }
}
