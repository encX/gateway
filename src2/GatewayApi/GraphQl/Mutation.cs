using HotChocolate.Subscriptions;

namespace GatewayApi.GraphQl;

[ExtendObjectType("Mutation")]
public class Mutation
{
    public async Task<TagPayload> AddTag(string name,
        [Service] ITagsApiClient service,
        [Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        await service.TagPOSTAsync(name, cancellationToken);
        var tag = new Tag { Name = name };
        await eventSender.SendAsync("AddTag", tag, cancellationToken);
        return new TagPayload { Tag = tag };
    }
}

public class TagPayload
{
    public Tag Tag { get; set; } = null!;
}
