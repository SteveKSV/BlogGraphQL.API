using EntityFramework.Data;
using EntityFramework.Entities;
using BlogGraphQL.API.Schema.DataLoaders;
using Microsoft.EntityFrameworkCore;
using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.Types;

namespace BlogGraphQL.API.NewFolder.Types
{
    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor
                .Field(t => t.Posts)
                .ResolveWith<UserResolvers>(t => t.GetUsersAsync(default!, default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("posts");

            descriptor
                .Field(t => t.Comments)
                .ResolveWith<UserResolvers>(t => t.GetUserCommentsAsync(default!, default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("comments")
                .Type<ListType<CommentType>>(); 

        }
        private class UserResolvers
        {
            public async Task<IEnumerable<User>> GetUsersAsync(
                User user, [ScopedService] BlogDbContext dbContext, 
                UserByIdDataLoader userById, CancellationToken cancellationToken)
            {
                Guid[] userIds = await dbContext.Users
                    .Where(a => a.Id == user.Id)
                    .Include(a => a.Posts)
                    .SelectMany(a => a.Posts.Select(t => t.Id))
                    .ToArrayAsync(cancellationToken: cancellationToken);

                return await userById.LoadAsync(userIds, cancellationToken);
            }

            public async Task<IEnumerable<Comment>> GetUserCommentsAsync(
                 [Parent] User user, [ScopedService] BlogDbContext dbContext,
                 CommentByIdDataLoader commentById, CancellationToken cancellationToken)
            {
                // Load the comments associated with the user
                var comments = await dbContext.Comments
                    .Where(comment => comment.AuthorId == user.Id)
                    .ToListAsync(cancellationToken);

                // Load the comment IDs
                var commentIds = comments.Select(comment => comment.Id).ToList();

                // Use DataLoader to load comments by ID
                var commentsById = await commentById.LoadAsync(commentIds, cancellationToken);

                return commentsById;
            }

        }
    }
}
