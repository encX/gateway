using GatewayApi.Models;

namespace GatewayApi.Queries;

public class Query
{
    public Tag GetTag()
    {
        return new Tag { Name = "HotChocolate" };
    }
}