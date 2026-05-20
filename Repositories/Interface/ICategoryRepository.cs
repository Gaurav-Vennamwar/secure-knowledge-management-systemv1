using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);//created a defination take a category insert in db and return the category

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id);
        Task<Category?> UpdateAsync(Category category);

    }
}
