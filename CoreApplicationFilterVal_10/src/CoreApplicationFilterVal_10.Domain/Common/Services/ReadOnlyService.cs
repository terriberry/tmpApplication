using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Domain.Common.Exceptions;
using CoreApplicationFilterVal_10.Domain.Common.Filters;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using CoreApplicationFilterVal_10.Domain.Contracts.Services;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;
using ValidationException = CoreApplicationFilterVal_10.Domain.Common.Exceptions.ValidationException;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.CRUD.ReadOnlyService", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Services;

public abstract class ReadOnlyService<TKey, TEntity, TViewModel> : IReadOnlyService<TKey, TEntity, TViewModel>
    where TEntity : class, IEntity
    where TViewModel : class
{
    private readonly IMapper _mapper;
    private readonly IReadOnlyRepository<TKey, TEntity> _repository;
    private readonly IValidator<PagedFilter> _pagedFilterValidator;

    protected ReadOnlyService(IReadOnlyRepository<TKey, TEntity> repository, IMapper mapper, IValidator<PagedFilter> pagedFilterValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _pagedFilterValidator = pagedFilterValidator;
    }

    public async ValueTask<TViewModel> Get(TKey key)
    {
        var entity = await _repository.Get(key);
        if (entity == null)
        {
            throw new NotFoundException(nameof(TEntity), $"{key}");
        }
        return _mapper.Map<TViewModel>(entity);
    }

    public async ValueTask<IEnumerable<TViewModel>> List()
    {
        var entities = await _repository.List();
        return entities.Select(m => _mapper.Map<TViewModel>(m));
    }

    public async ValueTask<IEnumerable<TViewModel>> List(ListFilter<TEntity> listFilter)
    {
        var filterExpression = listFilter.ToExpression();
        var entities = await _repository.List(filterExpression);
        return entities.Select(m => _mapper.Map<TViewModel>(m));
    }

    public async ValueTask<PagedList<TViewModel>> PagedList(PagedFilter pagedFilter, ListFilter<TEntity> listFilter = null)
    {
        var validationResult = await _pagedFilterValidator.ValidateAsync(pagedFilter);
        if (!validationResult.IsValid)
            throw new ValidationException("One or more validation errors", validationResult.Errors);
        var filterExpression = listFilter?.ToExpression();
        var entityPagedList = await _repository.PagedList(filterExpression, pagedFilter);

        var pagedList = new PagedList<TViewModel>
        {
            PageCount = entityPagedList.PageCount,
            PageSize = entityPagedList.PageSize,
            PageNo = entityPagedList.PageNo,
            TotalCount = entityPagedList.TotalCount,
            Items = _mapper.Map<List<TEntity>, List<TViewModel>>(entityPagedList.Items)
        };

        return pagedList;
    }
}
