using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.DataLoaders
{
    public class PostByIdDataLoader : BatchDataLoader<Guid, Post>
    {
        private readonly IDbContextFactory<BlogDbContext> _dbContextFactory;

        public PostByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BlogDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Post>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using BlogDbContext dbContext =
                _dbContextFactory.CreateDbContext();

            return await dbContext.Posts
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
