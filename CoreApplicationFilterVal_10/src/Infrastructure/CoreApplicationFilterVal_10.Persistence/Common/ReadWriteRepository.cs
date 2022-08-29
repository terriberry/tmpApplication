using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApplicationFilterVal_10.Domain.Common.Contracts;
using CoreApplicationFilterVal_10.Persistence.Persistence;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("TD.Repositories.EF.ReadWriteRepository", Version = "1.0")]

namespace CoreApplicationFilterVal_10.Persistence.Common;

public abstract class ReadWriteRepository<TKey, TEntity> : ReadOnlyRepository<TKey, TEntity>, IReadWriteRepository<TKey, TEntity>
    where TEntity : class, IEntity
{
    protected ReadWriteRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public virtual async Task<bool> Delete(Expression<Func<TEntity, bool>> filterExpression)
    {
        try
        {
            var list = Set.AsQueryable();
            if (filterExpression != null)
                list = list.Where(filterExpression);
            Set.RemoveRange(list);
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<bool> Delete(TKey key)
    {
        try
        {
            var entity = await Set.FindAsync(key);
            if (entity == null)
                return false;
            Set.Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        try
        {
            await Set.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities)
    {
        try
        {
            await Set.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return (List<TEntity>)entities;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }

    public virtual async Task<TEntity> Update(TKey key, TEntity entity)
    {
        try
        {
            var existing = await Set.FindAsync(key);
            Context.Entry(existing).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
            return existing;
        }
        catch (Exception ex)
        {
            throw new DataException(ex.Message, ex);
        }
    }
}
