using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Contracts.Repositories.ReadWriteRepositoryInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Contracts;

public interface IReadWriteRepository<in TKey, TEntity> : IReadOnlyRepository<TKey, TEntity>
    where TEntity : class, IEntity
{
    Task<bool> Delete(Expression<Func<TEntity, bool>> filterExpression);
    Task<bool> Delete(TKey key);
    Task<TEntity> Create(TEntity entity);
    Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities);
    Task<TEntity> Update(TKey key, TEntity entity);
}
