using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Repositories.Implementation;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;
using SecureKnowledgeManagementSystemv1.Repositories.Implementation;
using SecureKnowledgeManagementSystemv1.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
// Add services to container

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddHttpContextAccessor();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SKMSConnection"));
});
//auth db context
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SKMSConnection"));
});


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

//few configuration of code to tell the application what type of user we are using what type of role we are having and etc
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
     .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("SKMS")
     .AddEntityFrameworkStores<AuthDbContext>()
     .AddDefaultTokenProviders();

//identity options to define what kind of password we want to have
builder.Services.Configure<IdentityOptions>(options =>
{
    //password rule options
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});

//add the authentication and tokens
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:Key"]!))
        };
    });
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
        app.UseAuthentication();
        app.UseAuthorization();

        //for images to refresh
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
            RequestPath = "/Images"
        });

        app.MapControllers();

        app.Run();
    
    