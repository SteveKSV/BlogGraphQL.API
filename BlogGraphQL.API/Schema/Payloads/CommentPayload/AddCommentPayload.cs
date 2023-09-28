using BlogGraphQL.API.Schema.Payloads.PostPayload;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.CommentPayload
{
    public class AddCommentPayload : CommentPayloadBase
    {
        public AddCommentPayload(Comment comment) : base(comment)
        {
        }

        public AddCommentPayload(IReadOnlyList<Common.Error> errors) : base(errors)
        {
        }
    }
}