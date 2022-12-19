using Alten.Booking.Application.Abstractions;
using Alten.Booking.Domain.Abstractions;
using Moq;
using System.Linq.Expressions;

namespace Alten.Booking.Tests.Helpers
{
    public static class MockHelpers
    {
        public static Mock<T> GetRepositoryMock<T, U>(List<U> entities)
            where T : class, IRepository<U> where U : class, IEntity
        {
            var repository = new Mock<T>();

            // Get All
            _ = repository
                .Setup(r => r.Get())
                .Returns(entities.AsQueryable());
            // Get by Id (with includes)
            _ = repository
                .Setup(r => r.GetAsync(It.IsAny<string>(), It.IsAny<IEnumerable<Expression<Func<U, object>>>>()))
                .Returns((string id, IEnumerable<Expression<Func<U, object>>> includes) => Task.FromResult(entities?.Find(e => e.Id.Equals(id))));
            // Get with query filter
            _ = repository
                .Setup(r => r.Get(It.IsAny<IQuery<U>>()))
                .Returns((IQuery<U> query) => entities?.Where(query.Criteria().Compile()).AsQueryable());
            // Get with lambda filter (with includes)
            _ = repository
                .Setup(r => r.Get(It.IsAny<Expression<Func<U, bool>>>(), It.IsAny<IEnumerable<Expression<Func<U, object>>>>()))
                .Returns((Expression<Func<U, bool>> expression, IEnumerable<Expression<Func<U, object>>> includes) => entities.Where(expression.Compile()).AsQueryable());

            // Add
            _ = repository
                .Setup(r => r.AddAsync(It.IsAny<U>()))
                .Callback((U entity) => entities?.Add(entity));
            // Update (replace)
            _ = repository
                .Setup(r => r.Update(It.IsAny<U>()))
                .Callback((U entity) =>
                {
                    entities?.RemoveAll(e => e.Id.Equals(entity.Id));
                    entities?.Add(entity);
                });
            // Delete
            _ = repository
                .Setup(r => r.Remove(It.IsAny<U>()))
                .Callback((U entity) => entities?.Remove(entity));

            return repository;
        }
    }
}
