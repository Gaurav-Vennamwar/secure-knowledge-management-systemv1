using Microsoft.AspNetCore.Identity;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);//passing and user and roles -for claims 
    }
}
