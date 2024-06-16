using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using ShowTrack.Client.Services;
using System.Net;

namespace ShowTrack.Client.Extensions;

public static class WebAssemblyHostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddApiHttpClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddKeyedScoped<HttpClient>(AccountService.HTTP_CLIENT_NAME, (_, _) =>
        {
            var handler = new CookieHandler
            {
                InnerHandler = new HttpClientHandler()
            };
            return new(handler) { BaseAddress = new(builder.Configuration["BaseAddress"] ?? string.Empty) };
        });

        builder.Services.AddScoped(serviceProvider =>
        {
            var handler = new CookieHandler
            {
                InnerHandler = new UnauthorizedResponseHandler(serviceProvider.GetRequiredService<NavigationManager>())
                {
                    InnerHandler = new HttpClientHandler()
                }
            };

            return new HttpClient(handler)
            {
                BaseAddress = new(builder.Configuration["BaseAddress"] ?? string.Empty)
            };
        });
        return builder;
    }
}

file class CookieHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        return base.SendAsync(request, cancellationToken);
    }
}


file class UnauthorizedResponseHandler : DelegatingHandler
{
    private readonly NavigationManager _navigationManager;

    public UnauthorizedResponseHandler(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigationManager.NavigateTo("login");
        }

        return response;
    }
}