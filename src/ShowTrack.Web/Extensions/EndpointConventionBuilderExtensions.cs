namespace ShowTrack.Web.Extensions;

public static class EndpointConventionBuilderExtensions
{
    public static void ManageIdentityApi(this IEndpointConventionBuilder builder)
    {
        builder.AddEndpointFilter(new IdentityApiEndpointFilter());
    }
}

file sealed class IdentityApiEndpointFilter : IEndpointFilter
{
    private static readonly string[] _protectedEndpoints = ["/register"];

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.GetEndpoint() is not RouteEndpoint endpoint || !_protectedEndpoints.Contains(endpoint.RoutePattern.RawText))
        {
            return next.Invoke(context);
        }

        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated == false || !user.IsInRole("admin"))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            return ValueTask.FromResult(default(object));
        }

        return next.Invoke(context);
    }
}
