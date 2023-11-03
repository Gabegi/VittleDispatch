using System.Reflection;
using Futures.Api.Data;
using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(provider => builder.Configuration);
builder.Services.AddResponseCaching();

// Add db context
builder.Services.AddDbContext<FuturesContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//Registering Interfaces here --- Singleton,Scoped or Transient objects?
builder.Services.AddScoped<IFuturesContext, FuturesContext>();

// Add repositories 
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

// Enable response caching
app.UseResponseCaching();
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(30)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };

    await next();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();