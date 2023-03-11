using HotChocolate;
using System.Collections;

namespace GatewayApi;

[ExtendObjectType("Query")]
public class Query
{
    public async Task<ICollection<Tag>> GetTagsAsync(
        [Service] ITagsApiClient service,
        CancellationToken cancellationToken)
    {
        return await service.TagAllAsync(cancellationToken);
    }
    
    public Tag GetTag()
    {
        return new Tag { Name = "HotChocolate" };
    }
}