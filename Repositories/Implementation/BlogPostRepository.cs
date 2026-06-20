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
        //method to create a blog post
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        //method to delete blogpost by id 
        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var existingBlogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
           
            if(existingBlogPost != null)
            {
                dbContext .BlogPosts.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;    
            }
            return null;
        }

        //method to get all blog posts
        public async Task<IEnumerable<BlogPost>> GetAllAync(int pageNumber = 1, int pageSize = 10)
        {
            return await dbContext.BlogPosts.Include(x => x.Categories)
                 .Skip((pageNumber - 1) * pageSize) //if page 1 and size is 10 then = 1-1=0 show page from 1-10 skip 0
                 .Take(pageSize)
                 .ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await dbContext.BlogPosts.CountAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            //first fetch the blog post using the id
            //finding it
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            //checking for null
            if (existingBlogPost == null)
            {
                return null;
            }
            //if its not null then update blog post
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //Updateing Categories too
            existingBlogPost.Categories = blogPost.Categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        
    }
}