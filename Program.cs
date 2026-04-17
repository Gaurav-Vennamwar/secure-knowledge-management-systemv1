using Microsoft.EntityFrameworkCore;
using SecureKnowledgeMAnagementSystemv1.API.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Add services to container
builder.Services.AddControllers();

// Swagger support (.NET 8)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SKMSConnection"));
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
=======
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(Options =>
{
   Options.UseSqlServer(builder.Configuration.GetConnectionString("SKMS_KnowledgeDB"));
}) ;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
>>>>>>> c67f25997434f451cc2501a9dea6bdde534b32ab
}

app.UseHttpsRedirection();

<<<<<<< HEAD
app.UseAuthorization();

app.MapControllers();

app.Run();
=======
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
>>>>>>> c67f25997434f451cc2501a9dea6bdde534b32ab
