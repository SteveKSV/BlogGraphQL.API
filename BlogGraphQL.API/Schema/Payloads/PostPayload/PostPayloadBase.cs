using BlogGraphQL.API.Common;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.PostPayload
{
    public class PostPayloadBase : Payload
    {
        protected PostPayloadBase(Post post)
        {
            Post = post;
        }

        protected PostPayloadBase(IReadOnlyList<Common.Error> errors)
            : base(errors)
        {
        }

        public Post? Post { get; }
    }
}
