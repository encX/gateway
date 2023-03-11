namespace GatewayApi.GraphQl;

[ExtendObjectType("Subscription")]
public class Subscription
{
    [Subscribe]
    [Topic("AddTag")]
    public Tag TagAdded([EventMessage] Tag tag) => tag;
}
