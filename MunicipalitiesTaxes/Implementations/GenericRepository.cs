using Microsoft.EntityFrameworkCore;
using MunicipalitiesTaxes.Interfaces;
using System.Linq.Expressions;

namespace MunicipalitiesTaxes.Implementations
{
       public class GenericRepository<TContext, TEntity> :
        IGenericRepository<TContext, TEntity>
        where TEntity : class
        where TContext : DbContext
        {
            public GenericRepository(TContext entities)
            {
                this.entities = entities;
            }

            public virtual IQueryable<TEntity> GetAll()
            {
                IQueryable<TEntity> query = this.entities.Set<TEntity>();
                return query;
            }

            public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
            {
                var query = this.entities.Set<TEntity>().Where(predicate);
                return query;
            }

            public virtual void Add(TEntity entity)
            {
                this.entities.Set<TEntity>().Add(entity);
            }

            public virtual void Delete(TEntity entity)
            {
                this.entities.Set<TEntity>().Remove(entity);
            }

            public virtual void Update(TEntity entity)
            {
                this.entities.Entry(entity).State = EntityState.Modified;
            }

            public virtual void Save()
            {
                this.entities.SaveChanges();
            }

            public virtual async Task SaveAsync()
            {
                await this.entities.SaveChangesAsync();
            }

            private readonly TContext entities;
        }
}
