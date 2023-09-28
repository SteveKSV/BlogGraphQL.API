using BlogGraphQL.API.Schema.DataLoaders;
using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.Types
{
    public class CommentType : ObjectType<Comment>
    {
        protected override void Configure(IObjectTypeDescriptor<Comment> descriptor)
        {
            descriptor
                .Field(t => t.Post)
                .ResolveWith<CommentResolvers>(t => t.GetPostAsync(default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("post");

            descriptor
                .Field(t => t.Author)
                .ResolveWith<CommentResolvers>(t => t.GetAuthorAsync(default!, default!, default!, default))
                .UseDbContext<BlogDbContext>()
                .Name("author");

            descriptor
                .Field(t => t.AuthorId)
                .Ignore();

            descriptor
                .Field(t => t.PostId)
                .Ignore();
        }

        private class CommentResolvers
        {
            public async Task<Post> GetPostAsync(
                [Parent] Comment comment, [ScopedService] BlogDbContext dbContext,
                CancellationToken cancellationToken)
            {
                Post? post = await dbContext.Posts
                     .FirstOrDefaultAsync(post => post.Id == comment.PostId);

                if (post == null)
                {
                    throw new GraphQLException("Post_Of_Post_Is_Not_Found_Error");
                }

                return post;
            }

            public async Task<User> GetAuthorAsync(
                 [Parent] Comment comment,
                 [ScopedService] BlogDbContext dbContext,
                 UserByIdDataLoader userById,
                 CancellationToken cancellationToken)
            {
                User? user = await dbContext.Users
                    .FirstOrDefaultAsync(user => user.Id == comment.AuthorId);

                if (user == null)
                {
                    throw new GraphQLException("Author_Of_Comment_Is_Not_Found_Error");
                }

                return user;
            }
        }
    }
}
