using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public AuthController(NewsDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Dim_User user)
        {
            _context.Dim_User.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var user = await _context.Dim_User.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
            if (user == null)
                return Unauthorized();
            // Token generation logic (e.g., JWT) should go here.
            return Ok("Login successful");
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
