namespace BlogGraphQL.API.Schema.Inputs
{
    public record class UpdatePostInput (Guid postId, string Title, string Content);
}
