using Azure.Core;
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
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Tittle = request.Tittle,
                UrlHandle = request.UrlHandle,
            };

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            //covert domain model back to dto
            var response = new BlogPostDTO
            {
                id = blogPost.id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content= blogPost.Content,
                ShortDescription= blogPost.ShortDescription,
                FeaturedImageUrl= blogPost.FeaturedImageUrl,
                IsVisible= blogPost.IsVisible,
                Tittle= blogPost.Tittle,
                UrlHandle= blogPost.UrlHandle,
            };
            return Ok(response);


        }

        //GET : {apiBaseUrl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost()
        {
            var blogPosts = await blogPostRepository.GetAllAync();

            //convert domain model to dto
            var response = new List<BlogPostDTO>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDTO
                {
                    id = blogPost.id,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    Tittle = blogPost.Tittle,
                    UrlHandle = blogPost.UrlHandle,
                });
            }
            return Ok(response);
        }

    }
}
