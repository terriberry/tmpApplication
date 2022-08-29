using System.Collections.Generic;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Services.CRUD.ReadWriteServiceContract", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Contracts.Services;

public interface IReadWriteService<TKey, TEntity, TViewModel, TCreateModel, TUpdateModel> : IReadOnlyService<TKey, TEntity, TViewModel>
    where TViewModel : class
    where TCreateModel : class
    where TUpdateModel : class
{
    ValueTask<TViewModel> Create(TCreateModel createModel);
    ValueTask<IEnumerable<TViewModel>> Create(IEnumerable<TCreateModel> createModels);
    ValueTask<TViewModel> Update(TKey key, TUpdateModel updateModel);
    Task Delete(TKey key);
}
