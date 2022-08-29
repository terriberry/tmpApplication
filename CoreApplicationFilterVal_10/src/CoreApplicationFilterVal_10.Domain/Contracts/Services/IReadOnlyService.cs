using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Common.Filters;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using FluentValidation;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.CRUD.ReadOnlyServiceContract", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Contracts.Services;

public interface IReadOnlyService<TKey, TEntity, TViewModel>
    where TViewModel : class
{
    ValueTask<TViewModel> Get(TKey key);
    ValueTask<IEnumerable<TViewModel>> List();
    ValueTask<IEnumerable<TViewModel>> List(ListFilter<TEntity> listFilter);
    ValueTask<PagedList<TViewModel>> PagedList(PagedFilter pagedFilter, ListFilter<TEntity> listFilter = null, AbstractValidator<ListFilter<TEntity>> listFilterValidator = null);
}
