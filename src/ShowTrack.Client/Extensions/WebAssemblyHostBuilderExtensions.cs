using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ShowTrack.Client.Extensions;

public static class WebAssemblyHostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddApiHttpClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(_ => new HttpClient(new CookieHandler()) { BaseAddress = new(builder.Configuration["BaseAddress"] ?? string.Empty) });
        return builder;
    }
}

file class CookieHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return await base.SendAsync(request, cancellationToken);
    }
}