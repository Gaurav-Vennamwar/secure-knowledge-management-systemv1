using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostController (IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        //Post : {apiBaseUrl}/api/blogposts
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO request)
        {
            //convert dto to domain model
            var blogPost = new BlogPost
            {
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                content = request.content,
                ShortDescription = request.ShortDescription,
                FeaturedInageUrl = request.FeaturedInageUrl,
                IsVisible = request.IsVisible,
                tittle = request.tittle,
                UrlHandle = request.UrlHandle,
            };

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            //covert domain model back to dto
            var response = new BlogPostDTO
            {
                id = blogPost.id,
                Author = request.Author,
                PublishedDate = blogPost.PublishedDate,
                content= request.content,
                ShortDescription= request.ShortDescription,
                FeaturedInageUrl= request.FeaturedInageUrl,
                IsVisible= request.IsVisible,
                tittle= request.tittle,
                UrlHandle= request.UrlHandle,
            };
            return Ok(response);


        }
    }
}
