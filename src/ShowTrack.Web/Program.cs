using Microsoft.EntityFrameworkCore;
using ShowTrack.Data;
using ShowTrack.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dbProvider = builder.Configuration["DbProvider"];
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseDatabase(dbProvider, builder.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

await app.RunAsync();
