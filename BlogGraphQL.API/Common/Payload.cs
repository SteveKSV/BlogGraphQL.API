namespace BlogGraphQL.API.Common
{
    public abstract class Payload
    {
        protected Payload(IReadOnlyList<Error>? errors = null)
        {
            Errors = errors;
        }

        public IReadOnlyList<Error>? Errors { get; }
    }
}
