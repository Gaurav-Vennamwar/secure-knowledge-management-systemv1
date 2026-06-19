using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAync(int pageNumber = 1, int pageSize = 10);
        Task<int> GetCountAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task <BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);

    }

}