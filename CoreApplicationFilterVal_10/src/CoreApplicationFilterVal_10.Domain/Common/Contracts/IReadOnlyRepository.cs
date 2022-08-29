using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Domain.Contracts.Repositories.ReadOnlyRepositoryInterface", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Domain.Common.Contracts;

public interface IReadOnlyRepository<in TKey, TEntity> : IRepository
    where TEntity : class, IEntity
{
    Task<TEntity> Get(TKey key);
    Task<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression);
    Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> filterExpression);
    Task<IEnumerable<TEntity>> List();
    Task<PagedList<TEntity>> PagedList(Expression<Func<TEntity, bool>> filterExpression, PagedFilter pagedFilter);
}
