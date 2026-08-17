using Microsoft.AspNetCore.Mvc;
using JobTrackerApi.Models;
using JobTrackerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RegisterRequest = JobTrackerApi.Authentication.RegisterRequest;
using LoginRequest = JobTrackerApi.Authentication.LoginRequest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
//Husk at lave update password og fix at din database password er exposed
namespace JobTrackerApi.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase 
    {
        private readonly JobTrackerDbContext _context;

        public AuthController(JobTrackerDbContext context)
        {
            _context = context;
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
                return Ok();
                
            }

            var claims = new List<Claim>{ 
                new Claim(ClaimTypes.NameIdentifier,existingUser.Id.ToString())
            }; //måske lav en list emed lidt mere

            return Ok(Token);


        }

    }
}
