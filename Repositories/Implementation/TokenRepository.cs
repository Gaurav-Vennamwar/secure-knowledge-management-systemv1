using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            //creayte the claims from the roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };
            //enter more claims for the roles we have
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //cllaims ready now jwt token
            //jwt security token parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            //using key defining the credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //now defining token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience : configuration["Jwt:Audience"],
                claims : claims,
                expires : DateTime.Now.AddMinutes(15),
                signingCredentials : credentials);
            //return token
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
