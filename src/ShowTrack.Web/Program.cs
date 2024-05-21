using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShowTrack.Data;
using ShowTrack.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dbProvider = builder.Configuration["DbProvider"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseDatabase(dbProvider, builder.Configuration));

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddBearerToken(IdentityConstants.BearerScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<IdentityUser>().ManageIdentityApi();

await using var scope = app.Services.CreateAsyncScope();

await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

await scope.ServiceProvider.SeedData(app.Configuration);

await app.RunAsync();
