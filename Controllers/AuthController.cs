using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        //we are getting few methods which we can use to create users
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;//create and assign this field
            this.tokenRepository = tokenRepository;
        }

        //POST : {apiBaseUrl}/api/auth/login
        [HttpPost ("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            //everything is handled internally by the package identity package bro
            //check email exists or not
            var identityUser = await userManager.FindByEmailAsync(request.Email!);
            //check if its not null
            if (identityUser is not null)
            {
                //CHECK PASSWORD
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password!);
                //if check password result is succed then
                if (checkPasswordResult)
                {
                    //create a token for the user
                    var roles = await userManager.GetRolesAsync(identityUser);
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    //create a  reponse and add the jwt token
                    var reponse = new LoginResponseDTO()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        
                    };
                    Response.Cookies.Append("access_tokens", jwtToken, new CookieOptions
                    {
                        HttpOnly = true,//only http cokkie
                        Secure = true,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });
                }

            }
            ModelState.AddModelError("", "Email or password is incorrect");

            return ValidationProblem(ModelState);
        }


        //POST : {apivaseurl}/api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            //create identity user object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            //created the user
            var identityResult = await userManager.CreateAsync(user, request.Password!);
            //check if it succeded
            if (identityResult.Succeeded)
            {
                //asiging only the reader role
                //ass role to the useer
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                //again check it
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}
