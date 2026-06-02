using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task <BlogPost?> UpdateAsync(BlogPost blogPost);
    }

}