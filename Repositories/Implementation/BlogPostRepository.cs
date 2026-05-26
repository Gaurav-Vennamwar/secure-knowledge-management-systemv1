using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbContext;
        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }
    }
}
