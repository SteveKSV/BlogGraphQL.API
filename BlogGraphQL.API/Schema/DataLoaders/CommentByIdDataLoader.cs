using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.DataLoaders
{
    public class CommentByIdDataLoader : BatchDataLoader<Guid, Comment>
    {
        private readonly IDbContextFactory<BlogDbContext> _dbContextFactory;

        public CommentByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<BlogDbContext> dbContextFactory)
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory ??
                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Comment>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using BlogDbContext dbContext =
                _dbContextFactory.CreateDbContext();

            return await dbContext.Comments
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
