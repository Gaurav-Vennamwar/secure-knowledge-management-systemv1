using SecureKnowledgeMAnagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystem_v1.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);//created a defination take a category insert in db and return the category
    }
}
