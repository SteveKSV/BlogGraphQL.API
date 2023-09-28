using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.DataLoaders;
using EntityFramework.Data;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Queries
{
    [ExtendObjectType("Query")]
    public class PostQueries
    {
        [UseApplicationDbContext]
        public IQueryable<Post> GetPosts([ScopedService] BlogDbContext context) =>
               context.Posts;

        public Task<Post> GetPostByIdAsync(
           [ID(nameof(Post))] Guid id,
           PostByIdDataLoader dataLoader,
           CancellationToken cancellationToken) =>
           dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Post>> GetPostsByIdAsync(
          [ID(nameof(Post))] Guid[] ids,
          PostByIdDataLoader dataLoader,
          CancellationToken cancellationToken) =>
          await dataLoader.LoadAsync(ids, cancellationToken);

        public async Task<List<Post>> GetPostsByAuthorIdAsync(
            [ID(nameof(Post))] Guid authorId,
            [Service] PostByAuthorIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            var postsByAuthor = await dataLoader.LoadAsync(
                authorId,
                cancellationToken);

            return postsByAuthor;
        }
    }
}
