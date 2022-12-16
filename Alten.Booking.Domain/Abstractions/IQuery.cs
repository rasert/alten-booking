using System.Linq.Expressions;

namespace Alten.Booking.Domain.Abstractions
{
    public interface IQuery<T> where T : class, IEntity
    {
        Expression<Func<T, bool>> Criteria();
    }
}
