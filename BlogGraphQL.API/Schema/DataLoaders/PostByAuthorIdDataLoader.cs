using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.DataLoaders
{
    public class PostByAuthorIdDataLoader : BatchDataLoader<Guid, List<Post>>
    {
        private readonly IDbContextFactory<BlogDbContext> _dbContextFactory;

        public PostByAuthorIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BlogDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, List<Post>>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using BlogDbContext dbContext = _dbContextFactory.CreateDbContext();

            var postsByAuthor = await dbContext.Posts
                .Where(post => keys.Contains(post.AuthorId))
                .GroupBy(post => post.AuthorId)
                .ToDictionaryAsync(
                    group => group.Key,
                    group => group.ToList(),
                    cancellationToken);

            return postsByAuthor;
        }
    }
}
