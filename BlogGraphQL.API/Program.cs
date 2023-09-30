using BlogGraphQL.API.NewFolder.Types;
using BlogGraphQL.API.Schema.DataLoaders;
using BlogGraphQL.API.Schema.Mutations;
using BlogGraphQL.API.Schema.Queries;
using BlogGraphQL.API.Schema.Subscription;
using BlogGraphQL.API.Schema.Types;
using EntityFramework.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<BlogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<UserQueries>()
        .AddTypeExtension<PostQueries>()
        .AddTypeExtension<CommentQueries>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<UserMutations>()
        .AddTypeExtension<PostMutations>()
        .AddTypeExtension<CommentMutations>()
    .RegisterDbContext<BlogDbContext>(DbContextKind.Pooled)
    .AddType<UserType>()
    .AddType<PostType>()
    .AddType<CommentType>()
    .AddSubscriptionType<Subscriptions>()
    .AddDataLoader<UserByIdDataLoader>()
    .AddDataLoader<PostByIdDataLoader>()
    .AddDataLoader<PostByAuthorIdDataLoader>()
    .AddDataLoader<CommentByIdDataLoader>()
    .AddDataLoader<CommentByAuthorIdDataLoader>()
    .AddInMemorySubscriptions()
    ;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Add the following line to use routing middleware
app.UseWebSockets();
app.UseRouting();
app.UseCors("AllowReactApp");
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
