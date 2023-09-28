using BlogGraphQL.API.Extensions;
using BlogGraphQL.API.Schema.Inputs;
using BlogGraphQL.API.Schema.Payloads.UserPayload;
using EntityFramework.Data;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace BlogGraphQL.API.Schema.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutations
    {
        [UseApplicationDbContext]

        public async Task<AddUserPayload> RegisterUserAsync(
            AddUserInput input,
            [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken)
        {
            var user = new User
            {
                Nickname = input.Nickname,
                Email = input.Email,
            };

            context.Users.Add(user);

            await context.SaveChangesAsync(cancellationToken);

            return new AddUserPayload(user);
        }

        [UseApplicationDbContext]
        public async Task<User> UpdateUserNicknameAsync(Guid userId,
            string nickName, [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken
            )
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if ( user == null )
            {
                throw new GraphQLException($"There is not any user with id {userId}");
            }

            user.Nickname = nickName;

            context.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        [UseApplicationDbContext]
        public async Task<string> DeleteUserAsync(
            Guid userId, [ScopedService] BlogDbContext context,
            CancellationToken cancellationToken
            )
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new GraphQLException($"There is not any user with id {userId}");
            }

            List<Post> postsToDelete = context.Posts.Where(x=>x.AuthorId == userId).ToList();
            context.RemoveRange(postsToDelete);
            context.Remove<User>(user);

            await context.SaveChangesAsync(cancellationToken);

            return $"Successfully deleted user (and his posts) with id: {userId}";
        }
    }
}
