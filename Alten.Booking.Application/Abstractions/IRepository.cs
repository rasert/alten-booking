using Alten.Booking.Domain.Abstractions;
using System.Linq.Expressions;

namespace Alten.Booking.Application.Abstractions
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> GetAsync(string id, IEnumerable<Expression<Func<T, object>>>? includes = null);
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> expression, IEnumerable<Expression<Func<T, object>>>? includes = null);
        IQueryable<T> Get(IQuery<T> query);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
    }
}
