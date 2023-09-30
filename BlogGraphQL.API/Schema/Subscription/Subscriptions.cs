using BlogGraphQL.API.Schema.Mutations;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Subscription
{
    public class Subscriptions
    {
        [Subscribe]
        [Topic (nameof(CommentMutations.CreateCommentAsync))]
        public Comment OnCreatedComment([EventMessage] Comment createdComment)
            => createdComment;

        [Subscribe]
        [Topic(nameof(PostMutations.CreatePostAsync))]
        public Post OnCreatedPost([EventMessage] Post createdPost)
            => createdPost;
    }
}
