using BlogGraphQL.API.Common;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.CommentPayload
{
    public class CommentPayloadBase : Payload
    {
        protected CommentPayloadBase(Comment comment)
        {
            Comment = comment;
        }

        protected CommentPayloadBase(IReadOnlyList<Common.Error> errors)
            : base(errors)
        {
        }

        public Comment? Comment { get; }
    }
}
