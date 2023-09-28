namespace BlogGraphQL.API.Schema.Inputs
{
    public record class AddCommentInput(
        string Content, Guid AuthorId, Guid PostId
        );
}
