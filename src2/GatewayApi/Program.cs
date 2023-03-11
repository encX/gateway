using GatewayApi;
using GatewayApi.GraphQl;
using StackExchange.Redis;

const string AllowedOrigin = "allowedOrigin";

var builder = WebApplication.CreateBuilder(args);

var serviceSection = builder.Configuration.GetSection("Services");
var tagsApiEndpoint = serviceSection.GetValue<string>("TagsApi:endpoint");

builder.Services.AddHttpClient<ITagsApiClient, TagsApiClient>(client => client.BaseAddress = new Uri(tagsApiEndpoint));
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            name: AllowedOrigin,
            policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

// redis
(string endpoint, string password) redisConfiguration =
(
    serviceSection.GetValue<string>("Redis:endpoint"),
    serviceSection.GetValue<string>("Redis:password")
);

var likesapiEndpoint = serviceSection.GetValue<string>("LikesApi:endpoint");
builder.Services.AddHttpClient("LikesGqlClient", client
    => client.BaseAddress = new Uri(likesapiEndpoint));

// graphql
builder.Services
    .AddGraphQLServer()
    .AddTypeExtension<Query>()
    .AddTypeExtension<Mutation>()
    .AddTypeExtension<Subscription>()
    .AddRedisSubscriptions(_ =>
        ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { redisConfiguration.endpoint },
            Password = redisConfiguration.password
        }))
    .AddRemoteSchema("LikesGqlClient", ignoreRootTypes: false);

var app = builder.Build();
app.UseCors(AllowedOrigin);
app.UseWebSockets();
app.MapGraphQL();
app.Run();