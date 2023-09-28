namespace BlogGraphQL.API.Schema.Inputs
{
    public record class AddPostInput(string Title, string Content, Guid userId);
}
