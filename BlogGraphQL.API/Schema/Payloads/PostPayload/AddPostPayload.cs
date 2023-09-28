using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.PostPayload
{
    public class AddPostPayload : PostPayloadBase
    {
        public AddPostPayload(Post post) : base(post)
        {
        }

        public AddPostPayload(IReadOnlyList<Common.Error> errors) : base(errors)
        {
        }
    }
}
