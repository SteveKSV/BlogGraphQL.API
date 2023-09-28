using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.DataLoaders
{
    public class CommentByAuthorIdDataLoader : BatchDataLoader<Guid, List<Comment>>
    {
        private readonly IDbContextFactory<BlogDbContext> _dbContextFactory;

        public CommentByAuthorIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BlogDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Comment>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using BlogDbContext dbContext = _dbContextFactory.CreateDbContext();

            var commentsByAuthor = await dbContext.Comments
                .Where(comment => keys.Contains(comment.AuthorId))
                .GroupBy(comment => comment.AuthorId)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.ToList(),
                    cancellationToken);

            return commentsByAuthor;
        }
    }
}
