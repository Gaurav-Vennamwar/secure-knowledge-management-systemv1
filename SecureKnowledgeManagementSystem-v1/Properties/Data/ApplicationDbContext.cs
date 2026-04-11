using Microsoft.EntityFrameworkCore;

namespace SecureKnowledgeMAnagementSystemv1.API.Models.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<BlogPost> BlogPosts {get; set; }
        public DbSet<Category> Categories {get; set; }















        
    }
}