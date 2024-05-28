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
builder.Services.AddScoped<IShowsService, ShowsService>();

builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
