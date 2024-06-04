using StackExchange.Redis;
using NRedisStack.RedisStackCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Herxagon.MiniUrl.Api;
using Herxagon.MiniUrl.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();


// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>(); //app settings, env variables
    var redisConnectionString = configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(redisConnectionString);
});
builder.Services.AddSingleton<IStorageService>(sp =>
{
    IConnectionMultiplexer connection = sp.GetRequiredService<IConnectionMultiplexer>();
    var configuration = sp.GetRequiredService<IConfiguration>(); //app settings, env variables
    var expiration = configuration.GetValue<TimeSpan>("UrlExpiration");;
    
    return new RedisStore(connection.GetDatabase(0), connection.GetDatabase(1), expiration);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy", opt => opt
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();