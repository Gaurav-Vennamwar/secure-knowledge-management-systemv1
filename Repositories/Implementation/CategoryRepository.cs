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
        //creates new category
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;

        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory is null)
            {
                return null;
            }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        //gets all categegory
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        // This method updates an existing category in the database
        public async Task<Category?> UpdateAsync(Category category)
        {
            // Search the database for the category
            // whose id matches the incoming category id
            var existingCategory = await dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == category.Id);

            // Check if category actually exists in database
            if (existingCategory != null)
            {
                // Copy all updated values from the incoming category
                // object into the existing tracked database entity
                dbContext.Entry(existingCategory)
                         .CurrentValues
                         .SetValues(category);

                // Save changes permanently into database
                await dbContext.SaveChangesAsync();

                // Return the updated category object
                return existingCategory;
            }

            // If category was not found, return null
            return null;
        }
    } }
