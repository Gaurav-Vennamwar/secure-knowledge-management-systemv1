using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        //POST : {apibaseurl}/api/images
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
                    Tittle = tittle
                    DateCreated = DateTime.Now,
                };
            }
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
