using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.DataLoaders;
using EntityFramework.Data;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Queries
{
    [ExtendObjectType("Query")]
    public class CommentQueries
    {
        [UseApplicationDbContext]
        public IQueryable<Comment> GetComments([ScopedService] BlogDbContext context) =>
               context.Comments;

        public Task<Comment> GetCommentByIdAsync(
           [ID(nameof(Comment))] Guid id,
           CommentByIdDataLoader dataLoader,
           CancellationToken cancellationToken) =>
           dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Comment>> GetCommentsByIdAsync(
          [ID(nameof(Comment))] Guid[] ids,
          CommentByIdDataLoader dataLoader,
          CancellationToken cancellationToken) =>
          await dataLoader.LoadAsync(ids, cancellationToken);

        public async Task<List<Comment>> GetCommentsByAuthorIdAsync(
            [ID(nameof(Comment))] Guid authorId,
            [Service] CommentByAuthorIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            var postsByAuthor = await dataLoader.LoadAsync(
                authorId,
                cancellationToken);

            return postsByAuthor;
        }
    }
}

