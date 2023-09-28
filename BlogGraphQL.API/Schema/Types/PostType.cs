using BlogGraphQL.API.Schema.DataLoaders;
using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.Types
{
    public class PostType : ObjectType<Post>
    {
        protected override void Configure(IObjectTypeDescriptor<Post> descriptor)
        {
            descriptor
                .Field(t => t.Comments)
                .ResolveWith<PostResolvers>(t => t.GetPostCommentsAsync(default!, default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("comments")
                .Type<ListType<CommentType>>();

            descriptor
                .Field(t => t.Author)
                .ResolveWith<PostResolvers>(t => t.GetAuthorAsync(default!, default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("author");

            descriptor
                .Field(t => t.AuthorId)
                .Ignore();
        }

        private class PostResolvers
        {
            public async Task<IEnumerable<Comment>> GetPostCommentsAsync(
                 [Parent] Post post, [ScopedService] BlogDbContext dbContext,
                 CommentByIdDataLoader commentById, CancellationToken cancellationToken)
            {
                // Load the comments associated with the user
                var comments = await dbContext.Comments
                    .Where(comment => comment.PostId == post.Id)
                    .ToListAsync(cancellationToken);

                // Load the comment IDs
                var commentIds = comments.Select(comment => comment.Id).ToList();

                // Use DataLoader to load comments by ID
                var commentsById = await commentById.LoadAsync(commentIds, cancellationToken);

                return commentsById;
            }

            public async Task<User> GetAuthorAsync(
                [Parent] Post post,
                [ScopedService] BlogDbContext dbContext,
                UserByIdDataLoader userById,
                CancellationToken cancellationToken)
            {
                User? user = await dbContext.Users
                    .FirstOrDefaultAsync(user => user.Id == post.AuthorId);

                if (user == null)
                {
                    throw new GraphQLException("Author_Of_Post_Is_Not_Found_Error");
                }

                return user; 
            }

        }
    }
}
