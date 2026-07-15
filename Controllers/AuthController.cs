using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SecureKnowledgeManagementSystemv1.API.Models.DTO;
using SecureKnowledgeManagementSystemv1.API.Repositories.Interface;

namespace SecureKnowledgeManagementSystemv1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
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
                    
                    var roles = await userManager.GetRolesAsync(identityUser);

                    // Step 1: Create JWT
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    // Step 2: Create Refresh Token and save to DB
                    var refreshToken = await tokenRepository.GenerateRefreshTokenAsync(identityUser.Id);

                    // Remove cookies created before the cookie path was standardised to '/'.
                    // Without this, browsers can send a stale /api/auth refresh cookie first.
                    ClearLegacyAuthCookies();


                    // Step 3: Set JWT in HttpOnly cookie
                    Response.Cookies.Append("access_token", jwtToken, new CookieOptions
                    {
                        HttpOnly = true,//only http cokkie
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Path = "/",
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    });

                    // Step 4: Set Refresh Token in separate HttpOnly cookie
                    Response.Cookies.Append("refresh_token", refreshToken.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Path = "/",
                        Expires = DateTime.UtcNow.AddDays(7)
                    });

                    //create a  reponse and add the jwt token
                    var reponse = new LoginResponseDTO()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),

                    };
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
        public async Task <IActionResult> Logout()
        {

            // get refresh token from cookie
            var refreshToken = Request.Cookies["refresh_token"];

            // revoke it in DB if exists
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await tokenRepository.RevokeRefreshTokenAsync(refreshToken);
            }
            //this will over ride your previous cookie
            // expire both cookies
            Response.Cookies.Append("access_token", "", new CookieOptions
            {
                HttpOnly = true,//only http cokkie
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            Response.Cookies.Append("refresh_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            ClearLegacyAuthCookies();

            return Ok();
        }


        //refresh token rotation
        //POST : {apiBaseUrl}/api/auth/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // Step 1: Get refresh token from cookie
            var refreshTokenValue = Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshTokenValue))
            {
                return Unauthorized("No refresh token found");
            }

            // Step 2: Find refresh token in DB
            var refreshToken = await tokenRepository.GetRefreshTokenAsync(refreshTokenValue);

            // Step 3: Validate refresh token
            if (refreshToken == null ||
                refreshToken.IsRevoked ||
                refreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            // Step 4: Get user from DB
            var user = await userManager.FindByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Step 5: Get user roles
            var roles = await userManager.GetRolesAsync(user);

            // Step 6: Revoke OLD refresh token (rotation!)
            await tokenRepository.RevokeRefreshTokenAsync(refreshTokenValue);

            // Step 7: Generate NEW JWT
            var newJwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

            // Step 8: Generate NEW Refresh Token
            var newRefreshToken = await tokenRepository.GenerateRefreshTokenAsync(user.Id);

            // Step 9: Set NEW JWT cookie
            Response.Cookies.Append("access_token", newJwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            // Step 10: Set NEW Refresh Token cookie
            Response.Cookies.Append("refresh_token", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { message = "Tokens refreshed successfully" });
        }

        private void ClearLegacyAuthCookies()
        {
            var legacyCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/api/auth"
            };

            Response.Cookies.Delete("access_tokens", legacyCookieOptions);
            Response.Cookies.Delete("access_token", legacyCookieOptions);
            Response.Cookies.Delete("refresh_token", legacyCookieOptions);
        }
    }
    }
    
