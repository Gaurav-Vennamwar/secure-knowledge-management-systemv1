using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository ;
        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //GET : {apibaseURL}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repository to get all the images
            var images = await imageRepository.GetAll();
            //convert domain model to dto
            var response = new List<BlogImageDTO>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDTO {
                    Id = image.Id,
                    Tittle = image.Tittle,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });            
            }
            return Ok(response);
        }

        //POST : {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UpdateImage([FromForm] IFormFile file ,
            [FromForm] string fileName, [FromForm] string tittle)//getting the file imform from form data
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                //then we will upload file
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Tittle = tittle,
                    DateCreated = DateTime.Now,
                };
               blogImage = await imageRepository.Upload(file, blogImage);
                //convert domain model to dto
                var respomse = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Tittle = blogImage.Tittle,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url
                };


                return Ok(respomse);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            //to see if the file is from the allowed extensions
            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) )
            {
                //does not contain then add model error
                ModelState.AddModelError("file", "Unsupported File Format");
             }
            //to check the size of the file
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size Cannot be more than 10mb");
            }
        }
    }
}
