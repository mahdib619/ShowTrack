using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShowTrack.Data;
using ShowTrack.Web.Extensions;
using ShowTrack.Web.Models;
using ShowTrack.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailClient>(builder.Configuration.GetSection("EmailClient"));

builder.Services.AddCors(opt => opt.AddPolicy("AllowAll", config => config.SetIsOriginAllowed(_ => true)
                                                                          .AllowCredentials()
                                                                          .AllowAnyHeader()
                                                                          .AllowAnyMethod()));

var dbProvider = builder.Configuration["DbProvider"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseDatabase(dbProvider, builder.Configuration));

builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();

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

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<IdentityUser>().ManageIdentityApi();

app.UseSwagger();
app.UseSwaggerUI();

await using var scope = app.Services.CreateAsyncScope();

await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

await scope.ServiceProvider.SeedData(app.Configuration);

await app.RunAsync();
