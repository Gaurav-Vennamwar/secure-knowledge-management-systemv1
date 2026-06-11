using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost("login")]
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
                    return Ok(reponse);
                }

            }
            ModelState.AddModelError("", "Email or password is incorrect");

            return ValidationProblem(ModelState);
        }


        //POST : {apibaseurl}/api/auth/register
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


        //we will call this endpoint whenever refreshed
        //GET :{apiBaseUrl}/api/auth/me
        [Authorize]
        [HttpGet("me")]
        //it will extract the token information from the porogram.cs context then if
        //everything is valid it will then go in this method and extract the neccessary details
        public IActionResult UserDetails()
        {
            if(User.Identity == null || !User.Identity.IsAuthenticated){
                return Unauthorized();
            }

            var response = new LoginResponseDTO
            {
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList()
            };
            return Ok(response);
        }

        //logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //this will over ride your previous cookie
            Response.Cookies.Append("access_tokens", "", new CookieOptions
            {
                HttpOnly = true,//only http cokkie
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
            return Ok();
        }
    }
}
    
