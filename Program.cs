using Microsoft.EntityFrameworkCore;
using SecureKnowledgeMAnagementSystemv1.API.Models.Domain;

var builder = WebApplication.CreateBuilder(args);

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();