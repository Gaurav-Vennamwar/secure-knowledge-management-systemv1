using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.Repositories.Implementation;
using SecureKnowledgeManagementSystemv1.Repositories.Interface;
using SecureKnowledgeManagementSystemv1.API.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddControllers();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SKMSConnection"));
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();