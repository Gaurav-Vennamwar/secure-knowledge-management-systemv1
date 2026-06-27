using Microsoft.AspNetCore.Identity;
using SecureKnowledgeManagementSystemv1.API.Models.Domain;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);//passing and user and roles -for claims
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);//creates new refresh token, saves to DB

        Task<RefreshToken?> GetRefreshTokenAsync(string token);//finds refresh token in DB by token string

        Task RevokeRefreshTokenAsync(string token);//marks token as revoked on logout
    }
}
