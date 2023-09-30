using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.Inputs;
using BlogGraphQL.API.Schema.Payloads.CommentPayload;
using BlogGraphQL.API.Schema.Payloads.PostPayload;
using EntityFramework.Data;
using EntityFramework.Entities;
using HotChocolate.Subscriptions;

namespace BlogGraphQL.API.Schema.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class CommentMutations
    {
        [UseApplicationDbContext]

        public async Task<AddCommentPayload> CreateCommentAsync(
            AddCommentInput input,
            [ScopedService] BlogDbContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Content = input.Content,
                CreateAt = DateTime.UtcNow,
                AuthorId = input.AuthorId,
                PostId = input.PostId,
            };

            context.Comments.Add(comment);

            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(CreateCommentAsync), comment, cancellationToken);

            return new AddCommentPayload(comment);
        }
    }
}
