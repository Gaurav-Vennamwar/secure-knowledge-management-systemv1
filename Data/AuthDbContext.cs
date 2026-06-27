using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) :base(options)//to use options in program.cs
        {
            
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // this roles and and admin user gets inserted into db automatically Ef generates the sql for you runs on migration automatically
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "43844ffc-1789-4ed8-9517-82cd8de05057";
            var writterRoleId = "6361188-6b36-4d9b-8e7a-2e5319a4cf7c";

            //creating reader and writter role
            var roles = new List<IdentityRole>{
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writterRoleId,
                    Name = "Writter",
                    NormalizedName = "Writter".ToUpper(),
                    ConcurrencyStamp = writterRoleId

                }
            };
            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //create an admin user
            var adminUserId = "10a7620f-01e4-482c-a211-a52f503476a1";
            var admin = new IdentityUser()
            {
                Id= adminUserId,
                UserName = "admin",
                Email = "adminSKMS@gmail.com",
                NormalizedEmail = "adminSKMS@gmail.com",
                NormalizedUserName = "adminSKMS@gmail.com"
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin123");

            builder.Entity<IdentityUser>().HasData(admin);
            //give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writterRoleId,
                }
            };
            //seed the admin user
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
