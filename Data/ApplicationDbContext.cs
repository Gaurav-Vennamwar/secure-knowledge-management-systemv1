using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }



        public DbSet<SecureKnowledgeManagementSystemv1.API.Models.Domain.BlogPost> BlogPosts { get; set; }
        public DbSet<SecureKnowledgeManagementSystemv1.API.Models.Domain.Category> Categories { get; set; }
        public DbSet<SecureKnowledgeManagementSystemv1.API.Models.Domain.BlogImage> BlogImages { get; set; }
    
    }
}