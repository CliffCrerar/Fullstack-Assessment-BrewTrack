using BrewTrack.Data;
using BrewTrack.DataContext;
using BrewTrack.Helpers;
using BrewTrack.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

// Top Level Program Variables
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
string mySqlConnectionString = Ensure.ArgumentNotNull(configuration.GetConnectionString("MySql"));
string redisConnectionString = Ensure.ArgumentNotNull(configuration.GetConnectionString("Redis"));
builder.Logging.AddConsole();
// Add services to the container.
services.AddControllersWithViews();
services.AddDbContext<BrewTrackDbContext>(options => options.UseMySQL(mySqlConnectionString));
//Configure other services up here

services.AddSingleton<IConnectionMultiplexer>(options =>
{
    // using StackExchange.Redis;
    return ConnectionMultiplexer.Connect(redisConnectionString);
});
services.AddBrewTrackService(configuration);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app
    .MigrateDatabase()// migrate database before running application
    .Run();
