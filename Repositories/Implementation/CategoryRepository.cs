using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.Repositories.Interface;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.Repositories.Implementation
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

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.id == id);
        }
    }
}
