using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Domain.Common.Paging;
using CoreApplicationFilterVal_10.Persistence.Persistence;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Repositories.EF.ReadOnlyRepository", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence.Common;

public abstract class ReadOnlyRepository<TKey, TEntity> : IReadOnlyRepository<TKey, TEntity>
    where TEntity : class, IEntity
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<TEntity> Set;

    protected ReadOnlyRepository(ApplicationDbContext dbContext)
    {
        Context = dbContext;
        Set = dbContext.Set<TEntity>();
    }

    public virtual async Task<TEntity> Get(TKey key)
    {
        try
        {
            var entity = await Set.FindAsync(key);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
    {
        try
        {
            var entity = await Set.FirstOrDefaultAsync(filterExpression);
            if (entity != null)
                Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> List(Expression<Func<TEntity, bool>> filterExpression)
    {
        try
        {
            var entities = Set.AsQueryable();
            if (filterExpression != null)
                entities = Set.Where(filterExpression);
            return await entities.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> List()
    {
        try
        {
            return await Set.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<PagedList<TEntity>> PagedList(Expression<Func<TEntity, bool>> filterExpression, PagedFilter pagedFilter)
    {
        try
        {
            pagedFilter ??= new PagedFilter();
            var entities = Set.AsQueryable();

            if (filterExpression != null)
                entities = Set.Where(filterExpression);

            var totalCount = await entities.CountAsync();

            if (pagedFilter.OrderBy != null)
                entities = entities.OrderBy(pagedFilter.OrderBy);
            else if (pagedFilter.OrderByDesc != null)
                entities = entities.OrderBy($"{pagedFilter.OrderByDesc} desc");
            else
                entities = entities.OrderBy(Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Single().Name);

            var skip = (pagedFilter.PageNo - 1) * pagedFilter.PageSize;
            var entityList = await entities.Skip(skip).Take(pagedFilter.PageSize).ToListAsync();

            return new PagedList<TEntity>
            {
                Items = entityList,
                PageSize = pagedFilter.PageSize,
                PageNo = pagedFilter.PageNo,
                PageCount = (totalCount + pagedFilter.PageSize - 1) / pagedFilter.PageSize,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }
}
