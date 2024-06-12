using Coravel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShowTrack.Data;
using ShowTrack.Web.Extensions;
using ShowTrack.Web.Jobs;
using ShowTrack.Web.Models;
using ShowTrack.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScheduler();

builder.Services.Configure<EmailClient>(builder.Configuration.GetSection("EmailClient"));

builder.Services.AddCors(opt => opt.AddPolicy("AllowAll", config => config.SetIsOriginAllowed(_ => true)
                                                                          .AllowCredentials()
                                                                          .AllowAnyHeader()
                                                                          .AllowAnyMethod()));

var dbProvider = builder.Configuration["DbProvider"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseDatabase(dbProvider, builder.Configuration));

builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddTransient<NotifyShowsNewSeasonJob>();

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

app.Services.UseScheduler(schedule =>
{
    schedule.Schedule<NotifyShowsNewSeasonJob>()
            .DailyAt(ShowService.ShowsNotifyTime.Hour, ShowService.ShowsNotifyTime.Minute)
            .RunOnceAtStart();
});

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
