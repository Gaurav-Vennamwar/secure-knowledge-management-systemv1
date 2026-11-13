using System.Diagnostics.Metrics;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;
using SecureKnowledgeManagementSystemv1.API.Models.Wrappers;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;
using SecureKnowledgeManagementSystemv1.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }

        //Post : {apiBaseUrl}/api/blogposts
        [HttpPost]
        [Authorize(Roles = "Writter")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO request)
        {
            var urlHandle = CreateUrlHandle(request.UrlHandle, request.Tittle);

            if (await blogPostRepository.GetByUrlHandleAsync(urlHandle) is not null)
            {
                return Conflict(ApiResponse<BlogPostDTO>.FailureResponse("That URL handle is already in use. Choose a different one.", 409));
            }

            //convert dto to domain model
            var blogPost = new BlogPost
            {
                Author = request.Author ?? string.Empty,
                PublishedDate = request.PublishedDate == default ? DateTime.UtcNow : request.PublishedDate,
                Content = request.Content ?? string.Empty,
                ShortDescription = request.ShortDescription ?? string.Empty,
                FeaturedImageUrl = request.FeaturedImageUrl ?? string.Empty,
                IsVisible = request.IsVisible,
                Tittle = request.Tittle ?? string.Empty,
                UrlHandle = urlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);



            //covert domain model back to dto
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList(),
            };
            return Ok(ApiResponse<BlogPostDTO>.SuccessResponse(response, "BlogPost Created Successfully", 201));


        }

        //GET : {apiBaseUrl}/api/blogposts
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPost([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);
            var blogPosts = await blogPostRepository.GetAllAync(pageNumber, pageSize);
            var totalCount = await blogPostRepository.GetCountAsync();

            //convert domain model to dto
            var response = new List<BlogPostDTO>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    Content = null,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisible = blogPost.IsVisible,
                    Tittle = blogPost.Tittle,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }
            var paginatedResult = new
            {
                Items = response,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)//ceiling rounded up
            };

            return Ok(ApiResponse<object>.SuccessResponse(paginatedResult, "BlogPosts fetched successfully"));
        }


        // GET : {apiBaseUrl}/api/blogpost/category/c-sharp?pageNumber=1&pageSize=10

        [HttpGet("category/{urlHandle}")]
        public async Task<IActionResult> GetBlogPostsByCategory([FromRoute] string urlHandle, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var blogPosts = await blogPostRepository.GetByCategoryUrlHandleAsync(urlHandle, pageNumber, pageSize);
            var totalCount = await blogPostRepository.GetCountByCategoryUrlHandleAsync(urlHandle);
            var response = blogPosts.Select(blogPost => new BlogPostDTO
            {

                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = null,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                }).ToList()
            }).ToList();

            var paginatedResult = new
            {
                Items = response,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

               
            return Ok(ApiResponse<object>.SuccessResponse(paginatedResult, "Category blog posts fetched successfully"));
        }
        // GET : {apiBaseUrl}/api/blogpost/latest?count=4
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestBlogPosts([FromQuery] int count = 6)
        {
            var blogPosts = await blogPostRepository.GetLatestAsync(count);

            var response = blogPosts.Select(blogPost => new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = null,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            }).ToList();

            return Ok(
                ApiResponse<IEnumerable<BlogPostDTO>>.SuccessResponse(
                    response,
                    "Latest blog posts fetched successfully"
                )
            );
        }   

        //GET : {apiBaseUrl}/api/blogposts{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            //get the blog post from repo
            var blogPost = await blogPostRepository.GetByIdAsync(id);


            if (blogPost is null)
            {
                return NotFound(ApiResponse<BlogPostDTO>.FailureResponse("BlogPost not found", 404));
            }

            //else return ok reposne and conver domain model back to dto
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(ApiResponse<BlogPostDTO>.SuccessResponse(response, "BlogPost fetched successfully"));
        }

        //GET : {apiBaseUrl}/api/blogposts{urlHandle}
        [HttpGet("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound(ApiResponse<BlogPostDTO>.FailureResponse("BlogPost not found", 404));
            }
            //else return ok reposne and conver domain model back to dto
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(ApiResponse<BlogPostDTO>.SuccessResponse(response, "BlogPost fetched successfully"));

        }

        [Authorize(Roles = "Writter")]
        //PUT : {apiBaseUrl}/api/blogposts{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            var urlHandle = CreateUrlHandle(request.UrlHandle, request.Tittle);

            var matchingPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (matchingPost is not null && matchingPost.Id != id)
            {
                return Conflict(ApiResponse<BlogPostDTO>.FailureResponse("That URL handle is already in use. Choose a different one.", 409));
            }

            //convert dto to domain model
            var blogPost = new BlogPost
            {
                Id=id,
                Author = request.Author ?? string.Empty,
                PublishedDate = request.PublishedDate == default ? DateTime.UtcNow : request.PublishedDate,
                Content = request.Content ?? string.Empty,
                ShortDescription = request.ShortDescription ?? string.Empty,
                FeaturedImageUrl = request.FeaturedImageUrl ?? string.Empty,
                IsVisible = request.IsVisible,
                Tittle = request.Tittle ?? string.Empty,
                UrlHandle = urlHandle,
                Categories = new List<Category>()
            };
            //for each
            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                //check if existing category is not null
                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            //calling repository to update the blogpost domain model
            //which will update the blog post in db for us
           var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);

            if (updatedBlogPost is null)
            {
                return NotFound(ApiResponse<BlogPostDTO>.FailureResponse("BlogPost not found", 404));
            }

            //domain model back to dto converting
            var response = new BlogPostDTO
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                PublishedDate = blogPost.PublishedDate,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Tittle = blogPost.Tittle,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(ApiResponse<BlogPostDTO>.SuccessResponse(response, "BlogPost updated successfully"));
        }
        [Authorize(Roles = "Writter")]
        //DELETE : {apiBaseUrl}/api/blogposts{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(id);

            if (deletedBlogPost is null)
            {
                return NotFound(ApiResponse<BlogPostDTO>.FailureResponse("BlogPost not found", 404));
            }
            //convert domain model back to dto
            var response = new BlogPostDTO
            {
                Id = deletedBlogPost.Id,
                Author = deletedBlogPost.Author,
                PublishedDate = deletedBlogPost.PublishedDate,
                Content = deletedBlogPost.Content,
                ShortDescription = deletedBlogPost.ShortDescription,
                FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
                IsVisible = deletedBlogPost.IsVisible,
                Tittle = deletedBlogPost.Tittle,
                UrlHandle = deletedBlogPost.UrlHandle,
            };
            return Ok(ApiResponse<BlogPostDTO>.SuccessResponse(response, "BlogPost deleted successfully"));
        }

        private static string CreateUrlHandle(string? requestedHandle, string? title)
        {
            var wasProvided = !string.IsNullOrWhiteSpace(requestedHandle);
            var source = wasProvided ? requestedHandle! : title ?? string.Empty;
            var normalized = new string(source
                .Trim()
                .ToLowerInvariant()
                .Select(character => char.IsLetterOrDigit(character) ? character : '-')
                .ToArray())
                .Trim('-');

            if (string.IsNullOrWhiteSpace(normalized))
            {
                normalized = "post";
            }

            return wasProvided ? normalized : $"{normalized}-{Guid.NewGuid():N}";
        }
    }
    }

