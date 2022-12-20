using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Alten.Booking.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public async Task<T?> GetAsync(string id, IEnumerable<Expression<Func<T, object>>>? includes = null)
        {
            IQueryable<T> query = _entities;

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, property) => current.Include(property));
                return await query.SingleOrDefaultAsync(e => e.Id.Equals(id));
            }
            else
                return await _entities.FindAsync(id);
        }

        public IQueryable<T> Get()
        {
            return _entities.AsQueryable();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression, IEnumerable<Expression<Func<T, object>>>? includes = null)
        {
            IQueryable<T> query = _entities;

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, property) => current.Include(property));
                return query.Where(expression);
            }
            else
                return _entities.Where(expression);
        }

        public IQueryable<T> Get(IQuery<T> query)
        {
            return _entities.Where(query.Criteria());
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}
