using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystem_v1.Repositories.Interface;
using SecureKnowledgeMAnagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystem_v1.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;

        }
    }
}
