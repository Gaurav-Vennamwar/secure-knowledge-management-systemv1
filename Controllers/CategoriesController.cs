using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.Repositories.Interface;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;



namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;


        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //Post : https://localhost5251/api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDTO request)
        {
            //map dto to domain model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);

            //Domaion Model to DTO
            var response = new CategoryDTO
            {
                id = category.id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);

        }

        //Get : https://localhost5251/api/categories

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            //map model to dto buisness rule buddy
            var response = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDTO
                {
                    id = category.id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);

        }
        //Get : https://localhost5251/api/categories/{id}
        [HttpGet("{id:guid}")]
        
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }
            //else if we found it covert to the dto
            var response = new CategoryDTO
            {
                id = existingCategory.id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle

            };
            return Ok(response);
        }

        // PUT : https://localhost:5251/api/categories/{id}

        // This endpoint updates an existing category
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCategory(

            // /api/categories/123
            [FromRoute] Guid id, UpdateCategoryRequestDto request)

        // Get updated data from request 
        {
            // Convert DTO into Domain Model
            // DTO = data coming from frontend
            // Domain Model = actual database model/entity

            var category = new Category
            {
                // Use id from URL
                id = id,

                // Updated name from frontend
                Name = request.Name,

                // Updated url handle from frontend
                UrlHandle = request.UrlHandle
            };

            // Send updated category to repository
            // Repository handles actual database update logic
            category = await categoryRepository.UpdateAsync(category);

            // If repository returns null
            // means category was not found in database
            if (category == null)
            {
                return NotFound();
            }

            // Convert updated Domain Model back to DTO
            // because we should not directly expose database entities

            var response = new CategoryDTO
            {
                id = category.id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            // Return updated category to frontend
            return Ok(response);
        }


        // DELETE : https://localhost:5251/api/categories/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category == null) {
                return NotFound();
            }
            //then succes
            //convert domain model back to dto
            var reponse = new CategoryDTO
            {
                id = category.id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(reponse);

        }
    }
}
