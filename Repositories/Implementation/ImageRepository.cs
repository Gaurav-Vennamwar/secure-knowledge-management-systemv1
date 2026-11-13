using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Cloudinary cloudinary;
        private readonly bool isConfigured;

        public ImageRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;

            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            isConfigured = !string.IsNullOrWhiteSpace(cloudName) &&
                !string.IsNullOrWhiteSpace(apiKey) &&
                !string.IsNullOrWhiteSpace(apiSecret);

            var account = new Account(
                cloudName,
                apiKey,
                apiSecret
            );

            cloudinary = new Cloudinary(account);
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await dbContext.BlogImages
                .AsNoTracking()
                .OrderByDescending(image => image.DateCreated)
                .ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            if (!isConfigured)
            {
                throw new InvalidOperationException(
                    "Image storage is not configured. Add Cloudinary__CloudName, Cloudinary__ApiKey, and Cloudinary__ApiSecret to the API service environment variables.");
            }

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "skms-images",
                // Cloudinary public IDs cannot safely contain characters such as '#'.
                // Keep only a compact, URL-safe name before adding uniqueness.
                PublicId = $"{CreateSafePublicId(blogImage.FileName)}-{Guid.NewGuid():N}"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null || uploadResult.SecureUrl is null)
            {
                throw new InvalidOperationException(uploadResult.Error?.Message ?? "The image host did not return a URL.");
            }

            blogImage.Url = uploadResult.SecureUrl.ToString();

            await dbContext.BlogImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();

            return blogImage;
        }

        private static string CreateSafePublicId(string? fileName)
        {
            var safeName = new string((fileName ?? "image")
                .ToLowerInvariant()
                .Select(character => char.IsLetterOrDigit(character) ? character : '-')
                .ToArray())
                .Trim('-');

            return string.IsNullOrWhiteSpace(safeName) ? "image" : safeName;
        }
    }
}
