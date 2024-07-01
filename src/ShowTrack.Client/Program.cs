using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using ShowTrack.Client;
using ShowTrack.Client.Extensions;
using ShowTrack.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.AddApiHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AppAuthenticationStateProvider>());
builder.Services.AddScoped<AppAuthenticationStateProvider>();
builder.Services.AddScoped<IShowsService, ShowsService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();