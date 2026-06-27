using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureKnowledgeManagementSystemv1.API.Data;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly AuthDbContext authDbContext;

        public TokenRepository(IConfiguration configuration, AuthDbContext authDbContext)
        {
            this.configuration = configuration;
            this.authDbContext = authDbContext;
        }

        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            // generate a cryptographically secure random token string
            var tokenString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = tokenString,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // 7 days expiry
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            // save to DB
            await authDbContext.RefreshTokens.AddAsync(refreshToken);
            await authDbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await authDbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await authDbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await authDbContext.SaveChangesAsync();
            }
        }
    }
}