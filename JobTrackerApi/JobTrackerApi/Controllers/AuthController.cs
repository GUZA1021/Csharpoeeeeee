using JobTrackerApi.Data;
using JobTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = JobTrackerApi.Authentication.LoginRequest;
using RegisterRequest = JobTrackerApi.Authentication.RegisterRequest;

//Husk at lave update password
namespace JobTrackerApi.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase 
    {
        private readonly JobTrackerDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(JobTrackerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            bool userName = await _context.Users.AnyAsync(u => u.UserName == request.UserName);

            if (emailExists || userName) {
                return Conflict();
            }
       
            var newUser = new User(request.Email,request.UserName, "Temporary");
            var hasher = new PasswordHasher<User>();
            newUser.PasswordHash = hasher.HashPassword(newUser, request.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { newUser.Id, newUser.Email, newUser.UserName });

        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.InputEmail);
            if (existingUser == null)
            {
                return NotFound();
            }
            var hasher = new PasswordHasher<User>();
            var validationOfHash = hasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash,login.InputPassword);
            if (validationOfHash == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
                
            }

            if (validationOfHash == PasswordVerificationResult.SuccessRehashNeeded)
            {

                var newHash = hasher.HashPassword(existingUser, login.InputPassword);
                existingUser.PasswordHash = newHash;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }

            var claims = new List<Claim>{ 
                new Claim(ClaimTypes.NameIdentifier,existingUser.Id.ToString())
            }; //måske lav en list emed lidt mere
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: "JobTrackerApi", audience: "MyAudience", claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddMinutes(120), signingCredentials: signingCredentials); //ændrer det brug refresh tokens istedet
            var handleToken = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new { token = handleToken});

        }

    }
}
