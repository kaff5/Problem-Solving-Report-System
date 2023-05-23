using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web.Helpers;
using WEBBACK2.Exceptions;
using WEBBACK2.Models;
using WEBBACK2.Models.Auth;
using WEBBACK2.Models.Data;
using WEBBACK2.Models.RoleDir;
using WEBBACK2.Services;

namespace WEBBACK2.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthService service;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService service, ApplicationDbContext context)
        {
            this.service = service;
            _context = context;
        }


        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<TokenModel>> login(LoginDto model)
        {
            var identity = GetIdentity(model.userName, model.password);
            if (identity == null)
            {
                throw new Exception("Invalid username or password.");
            }

            var token = CreateToken(identity);
            var refreshToken = GenerateRefreshToken();

            await service.SaveToken(model.userName, refreshToken, DateTime.Now.AddDays(7));

            var response = new
            {
                accessToken = token,
                refreshToken = refreshToken,
            };

            return new JsonResult(response);
        }

        private static string CreateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfigurations.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }


        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _context.Users.Where(x => x.userName == username).FirstOrDefault();
            if (user is null)
            {
                throw new ValidationException("Wrong username or password");
            }
            
            if (Crypto.VerifyHashedPassword(user.password, password))
            {
                Role role = _context.Roles.Find(user.roleId);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.userName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.name)
                };
                var claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            throw new ValidationException("Wrong username or password");
        }


        [Route("logout")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await service.Logout(User.Identity.Name);
            return Ok();
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<TokenModel>> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) //Проверка полученной модели данных
            {
                throw new ValidationException("Bad data");
            }

            await service.Register(model);
            return await login(new LoginDto
            {
                userName = model.username,
                password = model.password
            });
        }
    }
}