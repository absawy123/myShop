using Microsoft.EntityFrameworkCore;
using myShop.DataAccess.Data;
using myShop.Entities.Repository;
using System.Linq.Expressions;

namespace myShop.DataAccess.Implementation
{
    // Basic crud operations for each model 

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> _dbset;
        public BaseRepository(AppDbContext context )
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public void Update(T entity)
        {
            _dbset.Attach(entity);  // Attach the entity if it's detached
            _context.Entry(entity).State = EntityState.Modified;  // Mark it as modified
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predecate =null, params string[] navProperties )
        {
            IQueryable<T> query = _dbset;
            if(predecate != null)
            {
                query= query.Where(predecate);
            }
            if(navProperties?.Length > 0)
            {
                foreach(var navProperty in navProperties)
                {
                    query = query.Include(navProperty);
                }
            }
            return query;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predecate =null, params string[] navProperties)
        {
            IQueryable<T> query = _dbset;
            if(predecate != null)
            {
                query = query.Where(predecate);
            }
            if (navProperties != null)
            {
                foreach(var navProperty in navProperties)
                {
                    query = query.Include(navProperty);
                }
            }
            return query.FirstOrDefault();

        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }
    }
}
