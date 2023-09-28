using EntityFramework.Entities;

namespace BlogGraphQL.API.Schema.Payloads.UserPayload
{
    public class AddUserPayload : UserPayloadBase
    {
        public AddUserPayload(User user) : base(user)
        {
        }

        public AddUserPayload(IReadOnlyList<Common.Error> errors) : base(errors)
        {
        }
    }
}
