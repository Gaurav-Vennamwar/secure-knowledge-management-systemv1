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
        public async Task<IActionResult> UpdateImage([FromForm] IFormFile? file ,
            [FromForm] string? fileName, [FromForm] string? tittle)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var safeFileName = string.IsNullOrWhiteSpace(fileName)
                    ? Path.GetFileNameWithoutExtension(file!.FileName)
                    : Path.GetFileNameWithoutExtension(Path.GetFileName(fileName.Trim()));

                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file!.FileName).ToLowerInvariant(),
                    FileName = safeFileName,
                    Tittle = string.IsNullOrWhiteSpace(tittle) ? safeFileName : tittle.Trim(),
                    DateCreated = DateTime.UtcNow,
                };
                try
                {
                    blogImage = await imageRepository.Upload(file, blogImage);
                }
                catch (InvalidOperationException exception)
                {
                    return Problem(
                        title: "Image upload could not be completed",
                        detail: exception.Message,
                        statusCode: StatusCodes.Status502BadGateway);
                }
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
        private void ValidateFileUpload(IFormFile? file)
        {
            if (file is null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Choose an image before uploading.");
                return;
            }

            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".webp" };

            //to see if the file is from the allowed extensions
            if(!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()) )
            {
                //does not contain then add model error
                ModelState.AddModelError("file", "Unsupported File Format");
             }
            //to check the size of the file
            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File Size Cannot be more than 10mb");
            }
        }
    }
}
