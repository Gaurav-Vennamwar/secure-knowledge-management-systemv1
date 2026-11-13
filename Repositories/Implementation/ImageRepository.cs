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

        public ImageRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;

            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
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
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "skms-images",
                // A unique public id prevents Cloudinary from rejecting uploads when
                // two assets share a human-friendly file name such as "cover".
                PublicId = $"{Path.GetFileNameWithoutExtension(blogImage.FileName)}-{Guid.NewGuid():N}"
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
    }
}
