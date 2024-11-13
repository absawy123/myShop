using System.Linq.Expressions;

namespace myShop.Entities.Repository
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll(Expression<Func<T,bool>> predecate =null, params string[] navProperties);
        T GetFirstOrDefault(Expression<Func<T, bool>> predecate =null, params string[] navProperties);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

    }
}
