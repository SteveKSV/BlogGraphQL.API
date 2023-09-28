using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.DataLoaders;
using EntityFramework.Data;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Queries
{
    [ExtendObjectType("Query")]
    public class UserQueries
    {
        [UseApplicationDbContext]
        public IQueryable<User> GetUsers([ScopedService] BlogDbContext context) =>
              context.Users;

        public Task<User> GetUserByIdAsync(
           [ID(nameof(User))] Guid id,
           UserByIdDataLoader dataLoader,
           CancellationToken cancellationToken) =>
           dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<User>> GetUsersByIdAsync(
          [ID(nameof(User))] Guid[] ids,
          UserByIdDataLoader dataLoader,
          CancellationToken cancellationToken) =>
          await dataLoader.LoadAsync(ids, cancellationToken);
    }
}
