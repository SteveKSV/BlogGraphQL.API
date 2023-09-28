using BlogGraphQL.API.Common;
using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.UserPayload
{
    public class UserPayloadBase : Payload
    {
        protected UserPayloadBase(User user)
        {
            User = user;
        }

        protected UserPayloadBase(IReadOnlyList<Common.Error> errors)
            : base(errors)
        {
        }

        public User? User { get; }
    }
}
