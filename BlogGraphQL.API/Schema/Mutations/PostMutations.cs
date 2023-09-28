using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.Inputs;
using BlogGraphQL.API.Schema.Payloads.PostPayload;
using BlogGraphQL.API.Schema.Payloads.UserPayload;
using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogGraphQL.API.Schema.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class PostMutations
    {
        [UseApplicationDbContext]

        public async Task<AddPostPayload> CreatePostAsync(
            AddPostInput input,
            [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Title = input.Title,
                Content = input.Content,
                AuthorId = input.userId
            };

            context.Posts.Add(post);

            await context.SaveChangesAsync(cancellationToken);

            return new AddPostPayload(post);
        }

        [UseApplicationDbContext]
        public async Task<Post> UpdatePostTitleAndContentAsync(
            UpdatePostInput input, [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken
            )
        {
            Post post = await context.Posts.FirstOrDefaultAsync(x => x.Id == input.postId);

            if (post == null)
            {
                throw new GraphQLException($"There is not any post with id {input.postId}");
            }

            post.Title = input.Title;
            post.Content = input.Content;

            context.Update(post);
            await context.SaveChangesAsync(cancellationToken);

            return post;
        }


        [UseApplicationDbContext]
        public async Task<string> DeletePostAsync(
            Guid postId, [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken
            )
        {
            Post post = await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                throw new GraphQLException($"There is not any post with id {postId}");
            }

            context.Remove<Post>(post);

            await context.SaveChangesAsync(cancellationToken);

            return $"Successfully deleted post with id: {postId}";
        }
    }
}
