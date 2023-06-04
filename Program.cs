using BrewTrack.Data;
using BrewTrack.DataContext;
using BrewTrack.Helpers;
using BrewTrack.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

// Top Level Program Variables
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
string mySqlConnectionString = Ensure.ArgumentNotNull(configuration.GetConnectionString("MySql"));
string redisConnectionString = Ensure.ArgumentNotNull(configuration.GetConnectionString("Redis"));
builder.Logging.AddConsole().AddDebug();

// Add services to the container.
services.AddControllersWithViews();
services.AddDbContext<BrewTrackDbContext>(options => options.UseMySQL(mySqlConnectionString));

//Configure other services up here

// Add Redis to service container
services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(options =>
{
    // using StackExchange.Redis;
    return ConnectionMultiplexer.Connect(redisConnectionString);
});
services.AddBrewTrackService(configuration);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


/* BUILD APP */ var app = builder.Build();


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
    /* MIGRATE APP */ .MigrateDatabase()
    /* RUN APP */ .Run();
