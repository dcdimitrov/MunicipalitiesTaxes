using System.Linq.Expressions;

namespace MunicipalitiesTaxes.Interfaces
{
    public interface IGenericRepository<TContext, TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        void Save();

        Task SaveAsync();
    }
}
