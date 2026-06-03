using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Implementation
{
    
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // 1 step - Upload the images to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            //uploading the file into physcial location 
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);          
            //2 step - Update the database
            //storing the blog image to database and getting the url
            //url could like be hhtps://skms.com/images/xyz.jpg
            var httpRequest = httpContextAccessor.HttpContext?.Request;
            var urlPath = $"{httpRequest?.Scheme}://{httpRequest?.Host}{httpRequest?.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

           await dbContext.BlogImages.AddAsync( blogImage );
            await dbContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
